using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [Header("Built object parent")]
    public GameObject BuiltObjectParent;
    [HideInInspector]
    public Buildable CurrentBuildable;
    private Buildable OldBuildable;
    private bool ConfirmedBuild = false;

    // Whenever something is built, this function can be called to add the object to it
    public void UpdateTile(Buildable NewBuildable)
    {
        OldBuildable = CurrentBuildable;
        CurrentBuildable = NewBuildable;
        if (BuiltObjectParent.transform.childCount > 0)
        {
            Destroy(BuiltObjectParent.transform.GetChild(0).gameObject);
        }

        if (NewBuildable.model != null || NewBuildable.type == BuildableType.Delete)
        {
            GameObject NewModel = Instantiate(NewBuildable.model, BuiltObjectParent.transform);
            print("Inactivated");
            NewModel.SetActive(false);
            NewModel.transform.Find("CollisionCheck").gameObject.SetActive(true);
            ConfirmedBuild = false;
        }
        else
        {
            Debug.LogError("No model given for chosen object");
        }
    }

    //The CollisionCheck object on all buildables will trigger this if objects are overlapping
    private void OnTriggerEnter(Collider other)
    {
        print("Collision");
        if (!ConfirmedBuild)
        {
            Destroy(BuiltObjectParent.transform.GetChild(0).gameObject);
            CurrentBuildable = OldBuildable;
        }
    }

    //Re-enables model after collision check
    private void FixedUpdate()
    {
        if (!ConfirmedBuild)
        {
            ConfirmedBuild = true;
            //Try catch because model does not exist on an empty tile
            try { BuiltObjectParent.transform.GetChild(0).gameObject.SetActive(true); } catch { }
        }
    }
}
