using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class NewTrainController : MonoBehaviour
{
    public float Speed = 10f;

    [Header("Turn Detector Origins")]
    public GameObject ForwardDetector;
    public GameObject BackwardDetector;

    [HideInInspector]
    public List<GameObject> ConnectedStock = new List<GameObject>();

    private void Update()
    {
        //Check the direction, if it moves in reverse, check the animator at the rear
        Transform DetectorOrigin = ForwardDetector.transform;

        if (Speed < 0) { DetectorOrigin = BackwardDetector.transform; }

        //Send a raycast to check for turns. If found, go through the turn
        RaycastHit hit;
        if (Physics.Raycast(DetectorOrigin.position, Vector3.down, out hit, 5f, LayerMask.GetMask("PathDetector")))
        {
            AnimationClip TurnAnimation = hit.transform.GetComponent<Animation>().GetClip("Turn");

            GetComponent<Animation>().AddClip(TurnAnimation, "Turning");
            GetComponent<Animation>().Play();
        }

        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Speed * Time.deltaTime);

    }
}
