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

    private void Start()
    {
        GH = GameObject.Find("ScriptHolder").GetComponent<GameHandler>();
    }

    // Whenever something is built, add the object to the tile
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
            GH.Savegame.tiles[int.Parse(gameObject.name)].builtObject = NewBuildable;
            if (NewBuildable.type == BuildableType.Track)
            {
                GH.Savegame.playerBalance -= 50;
            }
            else
            {
                GH.Savegame.playerBalance -= NewBuildable.price;
            }
        }
        else if (NewBuildable.type == BuildableType.Delete)
        {
            GH.Savegame.tiles[int.Parse(gameObject.name)].builtObject = NewBuildable;
            GH.Savegame.playerBalance -= NewBuildable.price;
        }
        else
        {
            Debug.LogError("No model given for chosen object");
        }
    }
}
