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
    private GameObject MouseModel;

    //Change the cursor to show what is selected
    private void Update()
    {
        GH.MoneyCounter.GetComponent<TextMeshProUGUI>().text = "$" + GH.Savegame.playerBalance;
        Vector3 MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y);
        GH.CursorWarning.transform.position = MousePos + new Vector3(10,-10,0);
        if (BuildMenuOpen)
        {
            MousePos = Camera.main.ScreenToWorldPoint(MousePos);
            MousePos = new Vector3(MousePos.x, 1.0f, MousePos.z);
            if (MouseModel != null)
            {
                MouseModel.transform.position = MousePos;
            }

            //Check if user pressed rotate button
            if (Input.GetButtonDown("Rotate"))
            {
                RotateBuildable(false);
            }
        }
    }

    //Generates a version of all UI elements so it can be used without needing to load it later
    public void GenerateUI()
    {
        foreach (Buildable Buildable in GH.Buildings)
        {
            if (Buildable.interactionUI != null)
            {
                Instantiate(Buildable.interactionUI);
            }
        }
    }

    //Opens the build options
    public void BuildOptions()
    {
        if (BuildMenuOpen) { GH.BuildMenu.SetActive(false); Destroy(MouseModel); }
        else { GH.BuildMenu.SetActive(true); }
        BuildMenuOpen = !BuildMenuOpen;
    }

    //Switches between the different menus
    public void SelectMenu(string Buildables)
    {
        //When a menu is selected, it takes all the neccasary data from the game handler.
        if (Buildables == "Track") { BuildableList = new List<Buildable>(GH.Tracks); }
        else if (Buildables == "Building") { BuildableList = new List<Buildable>(GH.Buildings); }
        else if (Buildables == "Delete") { BuildableList = new List<Buildable> { new Buildable("Delete",BuildableType.Delete,null,null,null,null,0) }; }
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

    //When a buildable is pressed, save which one was last chosen and spawn a model for the cursor
    public void SelectBuildable()
    {
        RotateBuildable(true);
        int Selected = int.Parse(EventSystem.current.currentSelectedGameObject.transform.parent.name);
        SelectedBuildable = new Buildable(BuildableList[Selected]);
        try { Destroy(MouseModel); } catch { }
        try { MouseModel = Instantiate(SelectedBuildable.model); } catch { }
    }

    public void RotateBuildable(bool Reset)
    {
        if (Reset && SelectedBuildable.model != null)
        {
            SelectedBuildable.model.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (SelectedBuildable.model != null)
        {
            SelectedBuildable.model.transform.rotation = Quaternion.Euler(0, SelectedBuildable.model.transform.rotation.eulerAngles.y + 90, 0);
            try { Destroy(MouseModel); } catch { }
            try { MouseModel = Instantiate(SelectedBuildable.model); } catch { }
        }
    }

    //function to display a warning on the cursor
    private Coroutine CursorWarningFunc = null;
    public void CursorWarning(string Message)
    {
        if (CursorWarningFunc != null)
        {
            StopCoroutine(CursorWarningFunc);
        }
        CursorWarningFunc = StartCoroutine(CursorWarningExec(Message));
    }

    private IEnumerator CursorWarningExec(string Message)
    {
        GH.CursorWarning.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = Message;
        GH.CursorWarning.SetActive(true);
        yield return new WaitForSeconds(2);
        GH.CursorWarning.SetActive(false);
    }
}
