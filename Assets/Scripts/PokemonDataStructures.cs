using UnityEngine;

[System.Serializable]
public class PokemonListResponse
{
    public Pokemon[] results;
}

[System.Serializable]
public class Pokemon
{
    public int pokemonIndex;
    public string name;
    public string url;
    public Sprite pokemonImage;
    public PokemonDetailedInfo pokemonDetailedInfo;
}
[System.Serializable]
public class PokemonDetailedInfo
{
    public int height;
    public int weight;
    public PokemonStat[] stats;
    public PokemonSpriteURL sprites;
}

[System.Serializable]
public class PokemonStat
{
    public int base_stat;
    public int effort;
    public NamedAPIResource stat;
}

[System.Serializable]
public class NamedAPIResource
{
    public string name;
    public string url;
}

[System.Serializable]
public class PokemonSpriteURL
{
    public string front_default;
}

