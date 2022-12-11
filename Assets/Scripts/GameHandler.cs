using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Temporary custom savegame instead of loading one from a file
    [Header("Temporary Savegame")]
    public Savegame SaveData = new Savegame(new Vector2(100, 100));

    //All variables that do not change during playtime will be set in the GameHandler to prevent repetition and to increase ease of change
    [Header("Objects")]
    public GameObject MapTiles;
    public GameObject MapBorders;

    [Header("Prefabs")]
    public GameObject BaseTile;
    public GameObject TileBorder;

    [Header("Loaded Savegame")]
    public Savegame Savegame;

    // Run everything neccesary to start the game
    private void Start()
    {
        // Savegame loading temporarily disabled until savegame system is completed
        Savegame = SaveData;

        //string RequestedSavegame = PlayerPrefs.GetString("LoadedSavegame");
        //Savegame Savegame = GetComponent<SavegameSystem>().GetSavegame(RequestedSavegame);

        // Generate tiles from savegame data
        GetComponent<MapGenerator>().GenerateMap(Savegame);
    }
}
