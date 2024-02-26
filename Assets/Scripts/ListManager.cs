using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListManager : MonoBehaviour
{

    public RectTransform Container;
    public Mask Mask;
    public PokemonCard Prefab;
    public int Num = 1;
    public int numColumns = 2;
    public Vector2 Spacing;

    private RectTransform maskRT;
    private int numVisible;
    private int numBuffer;
    private int bufferSize = 2;
    private float yContainerHalfSize;
    private float xContainerHalfSize;
    private float prefabHeight;
    private float prefabWidth;


    private Dictionary<int, int[]> itemDict = new Dictionary<int, int[]>();
    private List<RectTransform> listItemRect = new List<RectTransform>();
    private List<PokemonCard> pokemonCards = new List<PokemonCard>();
    private int numItems = 0;
    private Vector3 startPos;
    private PokemonListResponse response;

    private void Start()
    {
        StartUI.OnStartButtonClicked += RefreshList;
    }

    /// <summary>
    /// Init pokemon list
    /// </summary>
    /// <param name="pokemonListResponse"></param>
    public void InitializeList(PokemonListResponse pokemonListResponse)
    {
        response = pokemonListResponse;
        numBuffer = bufferSize * numColumns;
        Num = response.results.Length;
        Container.anchoredPosition3D = new Vector3(0, 0, 0);

        maskRT = Mask.GetComponent<RectTransform>();

        float widthInPixels = Container.rect.size.x - 3 * Spacing.x;

        Vector2 prefabScale = Prefab.rectTransform.rect.size;

        prefabHeight = prefabScale.y + Spacing.y;
        prefabWidth = widthInPixels / numColumns + Spacing.x;

        Container.sizeDelta = new Vector2(Container.sizeDelta.x, prefabHeight * (Num / numColumns));

        xContainerHalfSize = Container.rect.size.x * 0.5f;
        yContainerHalfSize = Container.rect.size.y * 0.5f;

        numVisible = Mathf.CeilToInt(maskRT.rect.size.y / prefabHeight) * numColumns;

        startPos = Container.anchoredPosition3D - (Vector3.down * yContainerHalfSize) - (Vector3.right * xContainerHalfSize) + (Vector3.down * (prefabScale.y * 0.5f) + (Vector3.right * (widthInPixels / numColumns * 0.5f)));
        numItems = Mathf.Min(Num, numVisible + numBuffer);
        int numRows = Mathf.CeilToInt(numItems / numColumns);
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numColumns; j++)
            {
                PokemonCard obj = Instantiate(Prefab, Container.transform);
                RectTransform t = obj.GetComponent<RectTransform>();
                t.sizeDelta = new Vector2(widthInPixels / numColumns, Prefab.rectTransform.sizeDelta.y);
                t.anchoredPosition3D = startPos + (Vector3.down * i * prefabHeight) + (Vector3.right * j * prefabWidth);
                listItemRect.Add(t);
                itemDict.Add(t.GetInstanceID(), new int[] { i, i });
                obj.gameObject.SetActive(true);
                pokemonCards.Add(obj);
                obj.UpdateContent(response.results[i * numColumns + j]);
            }
        }
        Prefab.gameObject.SetActive(false);
        Container.anchoredPosition3D += Vector3.down * (yContainerHalfSize - (maskRT.rect.size.y * 0.5f));
    }

    public void RefreshList()
    {
        for (int i = 0; i < pokemonCards.Count; i++)
        {
            pokemonCards[i].UpdateContent(response.results[i]);
        }
    }

    public void ReorderItemsByPos(float normPos)
    {
        if (numItems == 0) return;
        normPos = 1f - normPos;
        int numOutOfView = Mathf.CeilToInt(Mathf.Clamp01(normPos) / numColumns * (Num - numVisible)) * numColumns;   //number of elements beyond the top boundary
        int firstIndex = Mathf.Max(0, numOutOfView - numBuffer);   //index of first element beyond the top boundary
        int originalIndex = firstIndex % numItems;

        int newIndex = firstIndex;
        for (int i = originalIndex; i < numItems; i++)
        {
            MoveItemByIndex(listItemRect[i], newIndex);
            pokemonCards[i].UpdateContent(response.results[newIndex]);
            newIndex++;
        }
        for (int i = 0; i < originalIndex; i++)
        {
            MoveItemByIndex(listItemRect[i], newIndex);
            pokemonCards[i].UpdateContent(response.results[newIndex]);
            newIndex++;
        }
    }

    private void MoveItemByIndex(RectTransform item, int index)
    {
        int id = item.GetInstanceID();
        itemDict[id][0] = index;
        int xIndex = Mathf.CeilToInt(index / numColumns);
        int yIndex = index % numColumns;
        item.anchoredPosition3D = startPos + (Vector3.down * xIndex * prefabHeight) + (Vector3.right * yIndex * prefabWidth);
    }
}
