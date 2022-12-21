using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorldInteraction : MonoBehaviour
{
    [Header("Connect GameHandler")]
    public GameHandler GH;

    private void Update()
    {
        if (Input.GetMouseButton(0) && !GH.InBuildMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.transform.tag == "BuildingInteractor")
                {

                }   
            }
        }
    }
}
