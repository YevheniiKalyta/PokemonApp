using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;
using System.Text;
using DG.Tweening;

public class PokedexInitializer : MonoBehaviour
{
    public PokemonListResponse response { get; private set; }

    public ListManager listManager;
    StringBuilder stringBuilder;
    int startIndex;
    public Action<int> OnCounter;

    async void Start()
    {
        stringBuilder = new StringBuilder(Consts.pokemonListURL);
        startIndex = Consts.pokemonListURL.Length;

        await GetPokemonList(Consts.pokemonListURL + "?limit=1000", async () =>
        {
            listManager.InitializeList(response);
            await OnGetResponse();
        });

    }



    async Awaitable GetPokemonList(string url, Action OnDoneAction)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch Pokemon list: " + www.error + url);
                return;
            }

            response = JsonUtility.FromJson<PokemonListResponse>(www.downloadHandler.text);
            OnDoneAction?.Invoke();
        }
    }


    async Awaitable OnGetResponse()
    {
        for (int i = 0; i < response.results.Length; i++)
        {

            //if (destroyCancellationToken.IsCancellationRequested) return;

            response.results[i].pokemonIndex = i + 1;

            StringBuilder tmpString = new StringBuilder(response.results[i].url);
            tmpString.Remove(0, Consts.pokemonListURL.Length);
            stringBuilder.Append(tmpString);

            GetDetailedInfo(response.results[i], stringBuilder.ToString());

            Debug.Log(stringBuilder);
            stringBuilder.Remove(startIndex, tmpString.Length);
        }
    }


    int counter = 0;
    async Awaitable GetPokemonSprite(Pokemon pokemon, string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            UnityWebRequestAsyncOperation operation = www.SendWebRequest();
            await operation;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch sprite: " + www.error + url);
                return;
            }

            // Get the downloaded texture
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            texture.filterMode = FilterMode.Point;
            pokemon.pokemonImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            counter++;
            OnCounter(counter);
        }
    }



    async Awaitable GetDetailedInfo(Pokemon pokemon, string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            UnityWebRequestAsyncOperation operation = www.SendWebRequest();
            await operation;

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Failed to fetch sprite: " + www.error + url);
                return;
            }
            pokemon.pokemonDetailedInfo = JsonUtility.FromJson<PokemonDetailedInfo>(www.downloadHandler.text);

            await GetPokemonSprite(pokemon, pokemon.pokemonDetailedInfo.sprites.front_default);

        }
    }
}