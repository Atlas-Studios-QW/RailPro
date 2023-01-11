using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Connect GameHandler")]
    public GameHandler GH;

    /// <summary>
    /// Generates tiles from provided savegame. Only used when loading to initialise the game.
    /// </summary>
    /// <param name="LoadedSavegame">Currently loaded savegame data</param>
    public void GenerateMap(Savegame LoadedSavegame)
    {
        // ID for each tile to keep track of them when saving
        int CurrentID = 0;

        //Check if savegame is new or not
        bool ExistingSave = LoadedSavegame.tiles.Count > 0;

        //Create horizontal borders
        for (int y = 0; y < LoadedSavegame.mapSize.y + 1; y++)
        {
            GameObject HorizontalBorder = Instantiate(GH.TileBorder, new Vector3((LoadedSavegame.mapSize.x / 2) - 0.5f, 0, y - 0.5f), Quaternion.Euler(0, 90, 0), GH.MapBorders.transform);
            HorizontalBorder.transform.localScale = new Vector3(HorizontalBorder.transform.localScale.x, HorizontalBorder.transform.localScale.y, LoadedSavegame.mapSize.x);
        }

        // Spawn tiles all tiles and borders (1 extra time to make the final borders)
        for (int x = 0; x < LoadedSavegame.mapSize.x + 1; x++)
        {
            // Create vertical borders
            GameObject VerticalBorder = Instantiate(GH.TileBorder, new Vector3(x - 0.5f, 0, (LoadedSavegame.mapSize.y / 2) - 0.5f), Quaternion.Euler(0, 0, 0), GH.MapBorders.transform);
            VerticalBorder.transform.localScale = new Vector3(VerticalBorder.transform.localScale.x, VerticalBorder.transform.localScale.y, LoadedSavegame.mapSize.y);

            // Creating the tiles (skips last one to make final borders)
            if (x < LoadedSavegame.mapSize.x)
            {
                for (int y = 0; y < LoadedSavegame.mapSize.y; y++)
                {
                    // Instantiate a tile
                    GameObject NewTile = Instantiate(GH.BaseTile, new Vector3(x, 0, y), new Quaternion(0, 0, 0, 0), GH.MapTiles.transform);
                    NewTile.name = CurrentID.ToString();

                    if (ExistingSave)
                    {
                        //Existing savegame, load data from savegame data
                        NewTile.GetComponent<TileController>().UpdateTile(GH.Savegame.tiles[CurrentID].builtObject);
                    }
                    else
                    {
                        // New savegame, add empty tile
                        GH.Savegame.tiles.Add(new Tile(CurrentID, null));
                    }
                    CurrentID++;
                }
            }
        }

        //Place terrain which will be visible to player
        GameObject Terrain = Instantiate(GH.Terrain, GH.Level.transform);
        Terrain.transform.localScale = new Vector3(LoadedSavegame.mapSize.x, 0.1f, LoadedSavegame.mapSize.y);
        Terrain.transform.position = new Vector3(LoadedSavegame.mapSize.x / 2 - 0.5f, 0, LoadedSavegame.mapSize.y / 2 - 0.5f);
        GH.MapBorders.SetActive(false);
    }
}
