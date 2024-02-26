using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InfoCanvas : MonoBehaviour
{
    [SerializeField] private CanvasGroup infoPanelCG;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image pokemonImage;
    [SerializeField] private TextMeshProUGUI pokemonName;
    [SerializeField] private TextMeshProUGUI pokemonIndex;
    [SerializeField] private TextMeshProUGUI pokemonHeight;
    [SerializeField] private TextMeshProUGUI pokemonWeight;
    private int currentIndex;
    private bool isVisible;

    public void OnWebsiteClick()
    {
        Application.OpenURL(Consts.pokedexURL + currentIndex);


    }
    public void OnExitClick()
    {
        Application.Quit();
    }

    public void SetInfo(Pokemon pokemon)
    {
        if (!isVisible) ToggleVisibility();
        pokemonImage.sprite = pokemon.pokemonImage;
        pokemonName.text = pokemon.name;
        currentIndex = pokemon.pokemonIndex;
        pokemonIndex.text = currentIndex.ToString("0000");

        pokemonHeight.text = DecimetersToFeetAndInches(pokemon.pokemonDetailedInfo.height);
        pokemonWeight.text = HectogramsToPounds(pokemon.pokemonDetailedInfo.weight);
    }

    private void ToggleVisibility()
    {
        isVisible = !isVisible;
        infoPanelCG.alpha = isVisible ? 1 : 0;
        infoPanelCG.interactable = isVisible;
        infoPanelCG.blocksRaycasts = isVisible;
    }

    public static string DecimetersToFeetAndInches(float decimeters)
    {
        double totalInches = decimeters * 3.93700787;
        var feet = (int)(totalInches / 12);
        var inches = totalInches % 12;
        return feet.ToString("00") + "'" + inches.ToString("00");
    }
    public static string HectogramsToPounds(float hectograms)
    {
        return (hectograms * 0.22046226f).ToString("00.00") + "lbs";
    }
}
