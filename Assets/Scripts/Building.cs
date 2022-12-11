using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Connect GameHandler")]
    public GameHandler GH;

    private bool InBuildMode = false;

    public void SwitchBuildMode ()
    {
        if (InBuildMode) { ExitBuildMode(); }
        else { EnterBuildMode(); }
        InBuildMode = !InBuildMode;
    }

    private void EnterBuildMode()
    {
        GH.MapBorders.SetActive(true);
    }

    private void ExitBuildMode()
    {
        GH.MapBorders.SetActive(false);
    }
}
