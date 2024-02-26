using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class PokemonCard : MonoBehaviour
{
    public RectTransform rectTransform;
    public Image pokemonImage;
    public TextMeshProUGUI pokemonIndexText;
    public TextMeshProUGUI pokemonName;
    public Button button;
    Pokemon pokemon;
    public static Action<Pokemon> OnPokemonClicked;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    public void UpdateContent(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        pokemonIndexText.text = pokemon.pokemonIndex.ToString("0000");
        pokemonName.text = pokemon.name.ToUpper();
        pokemonImage.sprite = pokemon.pokemonImage ?? pokemonImage.sprite;
    }
    public void OnButtonClick()
    {
        OnPokemonClicked?.Invoke(pokemon);
    }
}
