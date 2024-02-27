using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class StartUI : MonoBehaviour
{
    [SerializeField] private PokedexInitializer pokedex;
    [SerializeField] private CanvasGroup startButtonCG;
    [SerializeField] private RectTransform boxDoor1, boxDoor2;
    [SerializeField] private RectTransform boxDoor1Target, boxDoor2Target;
    [SerializeField] private TextMeshProUGUI loadingTitle;
    [SerializeField] private Button startButton;

    private float downloadPortionToLetIn = 0.5f;
    private float animDuration = 1f;
    private Tween scaleTween;

    public static Action OnStartButtonClicked;

    private void Start()
    {

        startButton.interactable = false;
        pokedex.OnCounter += OnPokedexLoadCount;
    }

    private void OnPokedexLoadCount(int obj)
    {
        startButton.image.fillAmount = (float)obj / (pokedex.response.results.Length * downloadPortionToLetIn);
        if (startButton.image.fillAmount >= 1f)
        {
            pokedex.OnCounter -= OnPokedexLoadCount;
            startButton.interactable = true;
            scaleTween = startButton.transform.DOScale(1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            loadingTitle.gameObject.SetActive(false);
        }
    }

    public void OnStartClick()
    {
        SetCgInteractable(false);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(startButtonCG.DOFade(0f, animDuration).OnComplete(() => scaleTween.Kill()));
        sequence.Append(boxDoor1.DOMoveX(boxDoor1Target.position.x, 1f));
        sequence.Join(boxDoor2.DOMoveX(boxDoor2Target.position.x, 1f));

        OnStartButtonClicked?.Invoke();
    }

    private void SetCgInteractable(bool on)
    {
        startButtonCG.interactable = on;
        startButtonCG.blocksRaycasts = on;
    }
}
