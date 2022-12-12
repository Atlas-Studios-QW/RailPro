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

    public Savegame(Vector2 MapSize)
    {
        mapSize = MapSize;
        tiles = new List<Tile>();
    }
}

[System.Serializable]
public enum BuildableType
{
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