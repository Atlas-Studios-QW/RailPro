using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetMouseButtonDown(0))
        {
            if (InBuildMode)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    print(hit.transform.gameObject);
                }
            }
        }
    }
}