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

    private bool OnSpline = false;

    //Draw ray that will pick up colliders on track piece, then get the bezier curve that is attached
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Forward.transform.position, Vector3.down, out hit, 5f, LayerMask.GetMask("PathDetectors")))
        {
            BezierCurve NextSpline = hit.transform.GetComponent<BezierCurve>();

            if (NextSpline != null && !OnSpline)
            {
                StartCoroutine(FollowSpline(NextSpline));
            }
        }
    }

    private IEnumerator FollowSpline(BezierCurve Spline)
    {
        List<Vector3> Points = new List<Vector3>();
        for (float i = 0f; i < 1; i+=0.01f)
        {
            Points.Add(Spline.GetPointAt(i));
        }

        foreach (Vector3 Point in Points)
        {
            print(Point);
        }

        yield return null;
    }
}