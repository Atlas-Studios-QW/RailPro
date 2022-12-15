using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

// CustomClasses class kept in to stop unity from being a bitch about it
public class CustomClasses : MonoBehaviour {}

// Savegame class which stores all data that has to be saved.
[System.Serializable]
public class Savegame
{
    public Vector2 mapSize;
    public List<Tile> tiles;
    public int playerBalance;

    public Savegame() {
        mapSize = new Vector2(10,10);
        tiles = new List<Tile>();
        playerBalance = 100000;
    }

    public Savegame(Vector2 MapSize, int PlayerBalance)
    {
        mapSize = MapSize;
        tiles = new List<Tile>();
        playerBalance = PlayerBalance;
    }
}

[System.Serializable]
public enum BuildableType
{
    Delete,
    Track,
    Building
}

[System.Serializable]
public class Buildable
{
    public string name;
    public BuildableType type;
    public GameObject model;
    public Sprite icon;
    public string description;
    public int price;

    public Buildable(string Name, BuildableType Type, GameObject Model, Sprite Icon, string Description, int Price)
    {
        name = Name;
        type = Type;
        model = Model;
        icon = Icon;
        description = Description;
        price = Price;
    }
}

[System.Serializable]
public class Tile
{
    public int id;
    public Buildable builtObject;

    public Tile(int ID, Buildable BuiltObject)
    {
        id = ID;
        builtObject = BuiltObject;
    }
}