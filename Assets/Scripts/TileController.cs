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
        if (NewBuildable.model != null)
        {
            Instantiate(NewBuildable.model, BuiltObjectParent.transform);
        }
    }
}
