using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

// CustomClasses class kept in to stop unity from being a bitch about it
public class CustomClasses : MonoBehaviour {}

// Savegame class which stores all data that has to be saved.
public class Savegame
{
    public Vector2 mapSize; //?? Remove after generating for first time?

    public Savegame()
    {
        mapSize = new Vector2(0, 0);
    }

    public Savegame(Vector2 MapSize)
    {
        mapSize = MapSize;
    }
}
