using System.Collections;
using System.Collections.Generic;
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
        SplineRes = 1 / GH.SplineResolution;
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

                for (float i = 0f; i < 1; i += SplineRes)
                {
                    CurvePoints.Add(NextSpline.GetPointAt(i));
                }
                print("Calculated Points");

            }

        }

        if (CurvePoints.Count > 0 && !OnSpline)
        {
            OnSpline = true;
            StartCoroutine(FollowSpline(CurvePoints));
        }
    }

    private IEnumerator FollowSpline(List<Vector3> Points)
    {
        foreach (Vector3 Point in Points)
        {
            while (transform.position != Point)
            {
                transform.position = Vector3.MoveTowards(transform.position, Point, Speed * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }

        OnSpline = false;
        yield return null;
    }
}