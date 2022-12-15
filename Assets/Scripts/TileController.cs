using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [Header("Built object parent")]
    public GameObject BuiltObjectParent;
    [HideInInspector]
    public Buildable CurrentBuildable;

    // Whenever something is built, this function can be called to add the object to it
    public void UpdateTile(Buildable NewBuildable)
    {
        CurrentBuildable = NewBuildable;
        if (BuiltObjectParent.transform.childCount > 0)
        {
            Destroy(BuiltObjectParent.transform.GetChild(0).gameObject);
        }

        if (NewBuildable.model != null || NewBuildable.name == "Delete")
        {
            // Try catch because i told it to ignore it when it's null, but apparently it's still null sometimes and i dont want my console full of errors
            try { Instantiate(NewBuildable.model, BuiltObjectParent.transform); } catch { }
        }
        else
        {
            Debug.LogError("No model given for chosen object");
        }
    }
}
