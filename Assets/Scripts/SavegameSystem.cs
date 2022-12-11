using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SavegameSystem : MonoBehaviour
{
    /// <summary>
    /// Gets the savegame by entering the name of the document.
    /// (Data path: %persistentDataPath%/Savegames)
    /// </summary>
    /// <param name="SavegameName">The name of the document (without extension)</param>
    /// <returns>Class 'Savegame' : The read savegame at the specified file name</returns>
    public Savegame GetSavegame(string SavegameName)
    {
        string SavegamePath = Application.persistentDataPath + $"Savegames/{SavegameName}.txt";
        string ReadJSON = File.ReadAllText(SavegamePath);
        return JsonUtility.FromJson<Savegame>(ReadJSON);
    }
}
