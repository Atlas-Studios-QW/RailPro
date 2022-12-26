using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    [Header("Connect Objects")]
    public GameObject SplineStraight;
    public GameObject SplineTurn;
    public GameObject RailsTurn;

    private bool Switched = false;
    public void SwitchTrack()
    {
        if (Switched)
        {
            SplineStraight.SetActive(true);
            SplineTurn.SetActive(false);
            RailsTurn.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
        else
        {
            SplineStraight.SetActive(false);
            SplineTurn.SetActive(true);
            RailsTurn.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 3, 0);
        }
        Switched = !Switched;
    }
}
