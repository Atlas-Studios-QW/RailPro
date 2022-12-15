using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour
{
    [Header("Connect GameHandler")]
    public GameHandler GH;

    private bool InBuildMode = false;
    private Buildable WarningBuildable;
    private TileController WarningTile;
    private int PreviousTile;

    //Open/Close build mode, this will show and hide the borders on the map
    public void SwitchBuildMode()
    {
        if (InBuildMode) {
            GH.MapBorders.SetActive(false);
        }
        else {
            GH.MapBorders.SetActive(true);
        }
        InBuildMode = !InBuildMode;
    }

    //Checks if and which tile was pressed
    private void Update()
    {
        if (Input.GetMouseButton(0) && InBuildMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && !EventSystem.current.IsPointerOverGameObject())
            {
                //Check if player is hitting a different tile than before
                if (int.Parse(hit.transform.gameObject.name) != PreviousTile)
                {
                    Buildable SelectedBuildable = GetComponent<UIController>().SelectedBuildable;

                    if (SelectedBuildable != null)
                    {
                        TileController TC = hit.transform.gameObject.GetComponent<TileController>();

                        //If the current buildable is a building, warn the player before removing
                        if (TC.CurrentBuildable.type == BuildableType.Building)
                        {
                            WarningBuildable = SelectedBuildable;
                            WarningTile = TC;
                            GH.WarningBox.SetActive(true);
                        }
                        else
                        {
                            TC.UpdateTile(SelectedBuildable);
                        }
                    }
                    PreviousTile = int.Parse(hit.transform.gameObject.name);
                }
            }
        }
    }

    //If user tries to delete or replace a building, it will warn the player to confirm
    public void Warnbox(bool UserResponse)
    {
        GH.WarningBox.SetActive(false);
        if (UserResponse)
        {
            WarningTile.UpdateTile(WarningBuildable);
        }
        WarningBuildable = null;
        WarningTile = null;
    }
}