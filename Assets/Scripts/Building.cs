using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour
{
    [Header("Connect GameHandler")]
    public GameHandler GH;

    private bool InBuildMode = false;

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

    private void Update()
    {
        if (Input.GetMouseButton(0) && InBuildMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && !EventSystem.current.IsPointerOverGameObject())
            {
                Buildable SelectedBuildable = GetComponent<UIController>().SelectedBuildable;

                if (SelectedBuildable != null)
                {
                    hit.transform.gameObject.GetComponent<TileController>().UpdateTile(SelectedBuildable);
                }
            }
        }
    }
}