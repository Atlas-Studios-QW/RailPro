using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Connect GameHandler")]
    public GameHandler GH;
    [HideInInspector]
    public GameObject SelectedBuildable;
    private List<Buildable> BuildableList = new List<Buildable>();

    private bool BuildMenuOpen;

    public void BuildOptions()
    {
        if (BuildMenuOpen) { GH.BuildMenu.SetActive(false); }
        else { GH.BuildMenu.SetActive(true); }
        BuildMenuOpen = !BuildMenuOpen;
    }

    public void SelectMenu(string Buildables)
    {
        if (Buildables == "Track") { BuildableList = GH.Tracks; }
        else if (Buildables == "Building") { BuildableList = GH.Buildings; }
        else { Debug.LogError("Incorrect menu requested"); }

        int ID = 0;

        foreach (Transform OldBuildable in GH.BuildMenu.transform.Find("Buildables"))
        {
            Destroy(OldBuildable.gameObject);
        }

        foreach (Buildable Buildable in BuildableList)
        {
            GameObject NewIcon = Instantiate(GH.BuildableIconBase, GH.BuildMenu.transform.Find("Buildables"));
            NewIcon.transform.Find("ItemTitle").GetComponent<TextMeshProUGUI>().text = Buildable.name;
            NewIcon.transform.Find("Icon").GetComponent<Image>().sprite = Buildable.icon;
            NewIcon.name = ID.ToString();
            ID++;
        }
    }

    public void SelectBuildable()
    {
        int Selected = int.Parse(EventSystem.current.currentSelectedGameObject.transform.parent.name);
        print(BuildableList);
        print(Selected + ": " + BuildableList[Selected]);
    }
}
