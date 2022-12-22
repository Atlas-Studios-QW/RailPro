using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepotBehaviour : MonoBehaviour
{
    [Header("Game Handler")]
    public GameHandler GH;

    [Header("Connect UI")]
    public GameObject ScrollView;

    private void Start()
    {
        GH = GameObject.Find("ScriptHolder").GetComponent<GameHandler>();
    }
}
