using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TrainController : MonoBehaviour
{
    [Header("Game Handler")]
    public GameHandler GH;

    //Ray origin points
    [Header("Spline Detectors")]
    public GameObject Forward;
    public GameObject Reverse;

    public float Speed = 1;
    
    public bool OnSpline = false;
    private float SplineRes;

    private void Start()
    {
        GH = GameObject.Find("ScriptHolder").GetComponent<GameHandler>();
        SplineRes = 1 / GH.SplineResolution;
    }

    //Draw ray that will pick up colliders on track piece, then get the bezier curve that is attached
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Forward.transform.position, Vector3.down, out hit, 5f, LayerMask.GetMask("PathDetectors")))
        {
            BezierCurve NextSpline = hit.transform.GetComponent<BezierCurve>();

            if (NextSpline != null && !OnSpline)
            {
                OnSpline = true;
                StartCoroutine(FollowSpline(NextSpline));
            }
        }
    }

    private IEnumerator FollowSpline(BezierCurve Spline)
    {
        List<Vector3> Points = new List<Vector3>();
        for (float i = 0f; i < 1; i += SplineRes)
        {
            Points.Add(Spline.GetPointAt(i));
            print(SplineRes);
            yield return null;
        }

        foreach (Vector3 Point in Points)
        {
            while (transform.position != Point)
            {
                transform.position = Vector3.MoveTowards(transform.position, Point, Speed * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }

        yield return null;
    }
}