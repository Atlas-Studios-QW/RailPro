using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
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

    private void Update()
    {
        //Follow the next spline if it exists and it is not currently following one already
        if (CurvePoints.Count > 0 && !OnSpline)
        {
            OnSpline = true;
            StartCoroutine(FollowSpline(new List<Vector3>(CurvePoints)));
        }

        //Draw ray that will pick up colliders on track piece, then get the spline that is attached
        RaycastHit hit;
        if (Physics.Raycast(Forward.transform.position, Vector3.down, out hit, 5f, LayerMask.GetMask("PathDetectors")))
        {
            BezierCurve FoundSpline = hit.transform.GetComponent<BezierCurve>();

            if (FoundSpline != NextSpline)
            {
                print("Spline Found!");
                NextSpline = FoundSpline;

                //Get all points in curve based on the set resolution
                CurvePoints.Clear();
                for (float i = 0f; i < 1; i += 1 / SplineRes)
                {
                    CurvePoints.Add(NextSpline.GetPointAt(i));
                }
                print("Calculated Points");
            }
        }
    }

    //When called, will follow the by the selected points
    private IEnumerator FollowSpline(List<Vector3> Points)
    {
        float FinalRotation = 0;

        //Get the final point on the spline and check in which direction it is
        Vector3 FinalPoint = Points[Points.Count - 1];
        int Direction = DirectionCheck(Points);
        
        //If the direction is 0, meaning straight track, skip all the points in between the first and last and go straight towards the last (much more efficient)
        if (Direction == 0)
        {
            while (transform.position != FinalPoint)
            {
                transform.position = Vector3.MoveTowards(transform.position, FinalPoint, Speed * Time.deltaTime);
                yield return null;
            }
        }
        else
        {
            //Go past each point
            foreach (Vector3 Point in Points)
            {
                if (Points.IndexOf(Point) == 1 && FinalRotation == 0)
                {
                    FinalRotation = Mathf.Round((transform.rotation.eulerAngles.y + 45 * DirectionCheck(Points)) / 45) * 45;
                }
                //Rotate towards the next point gradually
                float RotateTarget = transform.rotation.eulerAngles.y + (45f * Direction / Points.Count);
                print("rotate: " + RotateTarget);
                transform.rotation = Quaternion.Euler(0, RotateTarget, 0);

                //Move towards next point
                while (transform.position != Point)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Point, Speed * Time.deltaTime);
                    yield return null;
                }
            }

            //Make sure the train is rotated correctly after turn
            print("Final Rotation: "+FinalRotation);
            transform.rotation = Quaternion.Euler(0, FinalRotation, 0);
        }
        OnSpline = false;
    }

    private int DirectionCheck(List<Vector3> Points)
    {
        if (Points[0].x == Points[Points.Count - 1].x || Points[0].z == Points[Points.Count - 1].z)
        {
            return 0;
        }
        else
        {
            //Gets direction towards point in degrees to check if it's left or right of the train
            Vector3 perp = Vector3.Cross(transform.forward, Points[Points.Count - 1] - transform.position);
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
}