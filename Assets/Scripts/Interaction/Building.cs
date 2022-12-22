using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour
{
    [Header("Connect GameHandler")]
    public GameHandler GH;

    private Buildable WarningBuildable;
    private TileController WarningTile;
    private int PreviousTile;

    //Open/Close build mode, this will show and hide the borders on the map
    public void SwitchBuildMode()
    {
        if (GH.InBuildMode) {
            GH.MapBorders.SetActive(false);
        }
        else {
            GH.MapBorders.SetActive(true);
        }
        GH.InBuildMode = !GH.InBuildMode;
    }

    //Checks if and which tile was pressed
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            PreviousTile = -2;
        }

        if (Input.GetMouseButton(0) && GH.InBuildMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("WorldTiles")) && !EventSystem.current.IsPointerOverGameObject())
            {
                //Check if player is hitting a different tile than before
                //Try catch to ignore anti build hitboxes
                int HitTileID = -1;
                try { HitTileID = int.Parse(hit.transform.gameObject.name); } catch { }
                if (HitTileID != PreviousTile && HitTileID != -1)
                {
                    //Check if the player selected a buildable
                    Buildable SelectedBuildable = GetComponent<UIController>().SelectedBuildable;

                    if (SelectedBuildable != null)
                    {
                        TileController TC = hit.transform.gameObject.GetComponent<TileController>();

                        //New buildable so it can be changed without causing the original to be changed
                        Buildable FinalBuildable = new Buildable(SelectedBuildable);

                        //Check if the selected buildable is not already there
                        if (FinalBuildable != TC.CurrentBuildable)
                        {
                            if (FinalBuildable.type == BuildableType.Delete)
                            {
                                FinalBuildable.price = -TC.CurrentBuildable.price / 2;
                            }
                            else if (FinalBuildable.type == BuildableType.Track && TC.CurrentBuildable.type == BuildableType.Track)
                            {
                                FinalBuildable.price = FinalBuildable.price / 2;
                            }

                            //If the current buildable is a building, warn the player before removing
                            if (TC.CurrentBuildable.type == BuildableType.Building && FinalBuildable.type == BuildableType.Delete)
                            {
                                WarningBuildable = SelectedBuildable;
                                WarningTile = TC;
                                GH.WarningBox.SetActive(true);
                            }
                            else
                            {
                                //Check if player has enough money, and if so, remove the cost, else warn the player
                                if (GH.Savegame.playerBalance >= FinalBuildable.price)
                                {
                                    TC.UpdateTile(FinalBuildable);
                                }
                                else
                                {
                                    GetComponent<UIController>().CursorWarning("Not enough money!");
                                }
                            }
                        }

                    }
                    PreviousTile = HitTileID;
                }
            }
        }
    }

    //If user tries to delete or replace a building, it will warn the player to confirm
    public void Warnbox(bool UserResponse)
    {
        GH.WarningBox.SetActive(false);
        if (UserResponse && GH.Savegame.playerBalance >= WarningBuildable.price)
        {
            GH.Savegame.playerBalance -= WarningBuildable.price;
            GH.Savegame.tiles[int.Parse(WarningTile.gameObject.name)].builtObject = WarningBuildable;
            WarningTile.UpdateTile(WarningBuildable);
        }
        WarningBuildable = null;
        WarningTile = null;
    }
}