using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Connect GameHandler")]
    public GameHandler GH;

    private bool BuildMenuOpen;

    public void BuildOptions()
    {
        if (BuildMenuOpen) { GH.BuildMenu.SetActive(false); }
        else { GH.BuildMenu.SetActive(true); }
        BuildMenuOpen = !BuildMenuOpen;
    }

    public void SelectMenu(string Buildables)
    {
        List<Buildable> BuildableList = new List<Buildable>();

        if (Buildables == "Track") { BuildableList = GH.Tracks; }
        else if (Buildables == "Building") { BuildableList = GH.Buildings; }
        else { print("INCORRECT MENU REQUESTED"); }

        foreach (Buildable Buildable in BuildableList)
        {
            print(Buildable.name);
        }
    }
}
