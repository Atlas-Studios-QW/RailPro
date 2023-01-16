using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Temporary custom savegame instead of loading one from a file
    [Header("Temporary Savegame")]
    public Savegame TempSaveData = new Savegame(new Vector2(100, 100), 100000);

    //All variables that do not change during playtime will be set in the GameHandler to prevent repetition and to increase ease of change
    [Header("Objects")]
    public GameObject Player;
    public GameObject Level;
    public GameObject MapTiles;
    public GameObject MapBorders;
    public GameObject TrainsParent;

    [Header("UI Objects")]
    public GameObject UICanvas;
    public GameObject BuildMenu;
    public GameObject WarningBox;
    public GameObject CursorWarning;
    public GameObject MoneyCounter;

    [Header("Prefabs")]
    public GameObject BaseTile;
    public GameObject TileBorder;
    public GameObject BuildableIconBase;
    public GameObject Terrain;

    [Header("Settings")]
    public float CameraSpeed = 10;
    public float SplineResolution = 10;

    [Header("Buildables")]
    public List<Buildable> Tracks;
    public List<Buildable> Buildings;

    [Header("Trains")]
    public List<Stock> Locomotives = new List<Stock>();
    public List<Stock> Traincars = new List<Stock>();

    [Header("Loaded Savegame")]
    public Savegame Savegame;

    [HideInInspector]
    public bool InBuildMode = false;

    // Run everything neccesary to start the game
    private void Start()
    {
        // Savegame loading temporarily disabled until savegame system is completed
        Savegame = TempSaveData;

        //string RequestedSavegame = PlayerPrefs.GetString("LoadedSavegame");
        //Savegame Savegame = GetComponent<SavegameSystem>().GetSavegame(RequestedSavegame);

        // Generate tiles from savegame data
        GetComponent<MapGenerator>().GenerateMap(Savegame);
    }
}
