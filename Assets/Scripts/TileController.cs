using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private GameHandler GH;

    [Header("Built object parent")]
    public GameObject BuiltObjectParent;
    [HideInInspector]
    public Buildable CurrentBuildable;
    private Buildable OldBuildable;
    private bool ConfirmedBuild = false;

    private void Start()
    {
        GH = GameObject.Find("ScriptHolder").GetComponent<GameHandler>();
    }

    // Whenever something is built, add the object to the tile
    public void UpdateTile(Buildable NewBuildable)
    {
        OldBuildable = CurrentBuildable;
        CurrentBuildable = NewBuildable;
        if (BuiltObjectParent.transform.childCount > 0)
        {
            Destroy(BuiltObjectParent.transform.GetChild(0).gameObject);
        }

        if (NewBuildable.model != null)
        {
            Instantiate(NewBuildable.model, BuiltObjectParent.transform);
            ConfirmedBuild = false;
            StartCoroutine(ConfirmBuild(NewBuildable));
        }
        else if (NewBuildable.type == BuildableType.Delete)
        {
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
        if (!ConfirmedBuild)
        {
            Destroy(BuiltObjectParent.transform.GetChild(0).gameObject);
            CurrentBuildable = OldBuildable;
            ConfirmedBuild = true;
            GH.GetComponent<UIController>().CursorWarning("Cannot build here!");
        }
    }

    //Confirms build when no collision is detected
    private IEnumerator ConfirmBuild(Buildable NewBuildabe)
    {
        yield return new WaitForSeconds(0.1f);
        if (!ConfirmedBuild)
        {
            ConfirmedBuild = true;
            GH.Savegame.tiles[int.Parse(gameObject.name)].builtObject = NewBuildabe;
            GH.Savegame.playerBalance -= NewBuildabe.price;
        }
    }
}
