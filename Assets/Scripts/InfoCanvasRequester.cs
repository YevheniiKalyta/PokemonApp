using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCanvasToggler : MonoBehaviour
{
    [SerializeField] private InfoCanvas infoCanvas;

    private void Start()
    {
        PokemonCard.OnPokemonClicked += ToggleInfoCanvas;
    }
    public void ToggleInfoCanvas(Pokemon pokemon = null)
    {
        infoCanvas.gameObject.SetActive(pokemon != null);
        if (pokemon != null)
        {
            infoCanvas.SetInfo(pokemon);
        }
    }
}




