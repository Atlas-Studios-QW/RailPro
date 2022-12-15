using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Connect GameHandler")]
    public GameHandler GH;
    [HideInInspector]
    public Buildable SelectedBuildable;
    private List<Buildable> BuildableList = new List<Buildable>();

    private bool BuildMenuOpen;

    //Opens the build options
    public void BuildOptions()
    {
        if (BuildMenuOpen) { GH.BuildMenu.SetActive(false); }
        else { GH.BuildMenu.SetActive(true); }
        BuildMenuOpen = !BuildMenuOpen;
    }

    //Switches between the different menus
    public void SelectMenu(string Buildables)
    {
        //When a menu is selected, it takes all the neccasary data from the game handler.
        if (Buildables == "Track") { BuildableList = new List<Buildable>(GH.Tracks); }
        else if (Buildables == "Building") { BuildableList = new List<Buildable>(GH.Buildings); }
        else if (Buildables == "Delete") { BuildableList = new List<Buildable> { new Buildable("Delete",BuildableType.Track,null,null,null,0) }; }
        else { Debug.LogError("Incorrect menu requested"); }

        int ID = 0;

        //Removes all buildables from the menu
        foreach (Transform OldBuildable in GH.BuildMenu.transform.Find("Buildables"))
        {
            Destroy(OldBuildable.gameObject);
        }

        //Places new buildables in the menu
        foreach (Buildable Buildable in BuildableList)
        {
            GameObject NewIcon = Instantiate(GH.BuildableIconBase, GH.BuildMenu.transform.Find("Buildables"));
            NewIcon.transform.position += new Vector3(100 * ID,0,0);
            NewIcon.transform.Find("ItemTitle").GetComponent<TextMeshProUGUI>().text = Buildable.name;
            NewIcon.transform.Find("Icon").GetComponent<Image>().sprite = Buildable.icon;
            NewIcon.transform.Find("Button").GetComponent<Button>().onClick.AddListener(SelectBuildable);
            NewIcon.name = ID.ToString();
            ID++;
        }
    }

    //When a buildable is pressed, save which one was last chosen
    public void SelectBuildable()
    {
        int Selected = int.Parse(EventSystem.current.currentSelectedGameObject.transform.parent.name);
        SelectedBuildable = BuildableList[Selected];
    }
}