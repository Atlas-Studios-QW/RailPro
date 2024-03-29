using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]

// CustomClasses class kept in to stop unity from giving me useless errors
public class CustomClasses : MonoBehaviour {}

// Savegame class which stores all data that has to be saved.
[System.Serializable]
public class Savegame
{
    public Vector2 mapSize;
    public List<Tile> tiles;
    public int playerBalance;

    public Savegame() 
    {
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
public enum StockType
{
    Locomotive,
    Traincar
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

    public Buildable(Buildable Buildable)
    {
        name = Buildable.name;
        type = Buildable.type;
        model = Buildable.model;
        icon = Buildable.icon;
        description = Buildable.description;
        price = Buildable.price;
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

[System.Serializable]
public class Stock
{
    public string name;
    public string description;
    public StockType type;
    public GameObject model;
    public Sprite icon;
    public int price;
    public int maxSpeed;
    public int trainEffect;

    public Stock(string Name, string Description, StockType Type, GameObject Model, Sprite Icon, int Price, int MaxSpeed, int TrainEffect)
    {
        name = Name;
        description = Description;
        type = Type;
        model = Model;
        icon = Icon;
        price = Price;
        maxSpeed = MaxSpeed;
        trainEffect = TrainEffect;
    }
}

[System.Serializable]
public class StockPostition
{
    public Vector3 position;
    public Quaternion rotation;

    public StockPostition(GameObject Object)
    {
        position = Object.transform.position;
        rotation = Object.transform.rotation;
    }
}

[System.Serializable]

public class Consist
{
    public List<Stock> stock;
    public List<StockPostition> positions;

    public Consist()
    {
        stock = new List<Stock>();
        positions = new List<StockPostition>();
    }
}