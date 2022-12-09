using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject Player;
    void Update()
    {
        Input.GetAxis("horizontal");
    }
}
