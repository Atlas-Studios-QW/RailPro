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
    public GameObject MapTiles;
    public GameObject MapBorders;

    [Header("UIObjects")]
    public GameObject UICanvas;
    public GameObject BuildMenu;
    public GameObject WarningBox;
    public GameObject CursorWarning;
    public GameObject MoneyCounter;

    [Header("Prefabs")]
    public GameObject BaseTile;
    public GameObject TileBorder;
    public GameObject BuildableIconBase;

    [Header("Settings")]
    public float CameraSpeed = 10;
    public int SplineResolution = 10;

    [Header("Buildables")]
    public List<Buildable> Tracks;
    public List<Buildable> Buildings;

    [Header("Loaded Savegame")]
    public Savegame Savegame;

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
