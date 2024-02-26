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

    [SerializeField] private Image pokemonImage;
    [SerializeField] private TextMeshProUGUI pokemonIndexText;
    [SerializeField] private TextMeshProUGUI pokemonName;
    [SerializeField] private Button button;

    private Pokemon pokemon;

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
