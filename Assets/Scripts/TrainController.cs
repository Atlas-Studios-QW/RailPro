using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using TreeEditor;
using Unity.PlasticSCM.Editor.WebApi;
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

    public BezierCurve NextSpline;
    List<Vector3> CurvePoints = new List<Vector3>();

    public bool OnSpline = false;
    public float SplineRes;

    private void Start()
    {
        GH = GameObject.Find("ScriptHolder").GetComponent<GameHandler>();
        SplineRes = GH.SplineResolution;
    }

    //Draw ray that will pick up colliders on track piece, then get the bezier curve that is attached
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Forward.transform.position, Vector3.down, out hit, 5f, LayerMask.GetMask("PathDetectors")))
        {
            BezierCurve FoundSpline = hit.transform.GetComponent<BezierCurve>();

            if (FoundSpline != NextSpline)
            {
                print("Spline Found!");
                NextSpline = FoundSpline;

                CurvePoints.Clear();
                for (float i = 0f; i < 1; i += 1 / SplineRes)
                {
                    CurvePoints.Add(NextSpline.GetPointAt(i));
                }
                print("Calculated Points");
            }
        }

        if (CurvePoints.Count > 0 && !OnSpline)
        {
            OnSpline = true;
            StartCoroutine(FollowSpline(new List<Vector3>(CurvePoints)));
        }
    }

    private IEnumerator FollowSpline(List<Vector3> Points)
    {
        bool First = true;
        foreach (Vector3 Point in Points)
        {
            if (First)
            {
                First = false;
                float FinalRotation = transform.rotation.eulerAngles.y + (45 * DirectionCheck(Points[Points.Count - 1]));
            }
            int Direction = DirectionCheck(Point);
            float RotateTarget = transform.rotation.eulerAngles.y + (45 * Direction / (Points.Count / 2));
            transform.rotation = Quaternion.Euler(0,RotateTarget,0);

            while (transform.position != Point)
            {
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,RotateTarget,0), Speed * 10 * Time.deltaTime);
                transform.position = Vector3.MoveTowards(transform.position, Point, Speed * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, FinalRotation, 0);

        yield return null;
        OnSpline = false;
    }

    private int DirectionCheck(Vector3 Point)
    {
        Vector3 perp = Vector3.Cross(transform.forward, Point - transform.position);
        float dir = Vector3.Dot(perp, transform.up);

        if (dir > 0.01f)
        {
            return 1;
        }
        else if (dir < -0.01f)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
}