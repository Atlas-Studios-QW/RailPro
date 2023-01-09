using System;
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
        if (Input.GetMouseButtonDown(0) && !GH.InBuildMode)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("WorldInteractors")) && !EventSystem.current.IsPointerOverGameObject())
            {
                print("World interaction hit: "+hit.transform.gameObject);
                try { hit.transform.Find("InteractionUI").gameObject.SetActive(true); } catch (Exception Error) { print(Error); }
                try { hit.transform.Find("BuiltObject").GetChild(0).GetComponent<SwitchController>().SwitchTrack(); } catch { }
            }
        }
    }
}
