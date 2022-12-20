using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TrainController : MonoBehaviour
{
    //Ray origin points
    [Header("Spline Detectors")]
    public GameObject Forward;
    public GameObject Reverse;

    public float Speed = 1;

    //Draw ray that will pick up colliders on track piece, then get the bezier curve that is attached
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Forward.transform.position, Vector3.down, out hit, 5f, LayerMask.GetMask("PathDetectors")))
        {

        }
    }
}