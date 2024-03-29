using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    [Header("Game Handler")]
    public GameHandler GH;

    //Ray origin points
    [Header("Spline Detectors")]
    public GameObject Forward;
    public GameObject Reverse;

    [HideInInspector]
    public List<GameObject> ConnectedStock = new List<GameObject>();

    public bool Direction = true;
    public float Speed = 10;
    public float RequestedSpeed = 10;

    [HideInInspector]
    public Stock StockInfo;
    [HideInInspector]
    public BezierCurve NextSpline;
    private List<Vector3> CurvePoints = new List<Vector3>();
    [HideInInspector]
    public bool OnSpline = false;
    [HideInInspector]
    public List<Vector3> FollowerLocations = new List<Vector3>();
    private float SplineRes;

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

        print(ConnectedStock[0]);

        if (ConnectedStock[0] == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(Forward.transform.position, Vector3.down, out hit, 5f, LayerMask.GetMask("PathDetectors")))
            {
                BezierCurve FoundSpline = hit.transform.GetComponent<BezierCurve>();

                print("Found Spline");

                if (FoundSpline != NextSpline)
                {
                    NextSpline = FoundSpline;

                    //Get all points in curve based on the set resolution
                    CurvePoints.Clear();
                    for (float i = 0f; i < 1; i += 1 / SplineRes)
                    {
                        CurvePoints.Add(NextSpline.GetPointAt(i));
                    }
                }
            }

            Speed = UpdateSpeed(RequestedSpeed);

        }
        else
        {

        }
    }

    //When called, will follow the by the selected points
    private IEnumerator FollowSpline(List<Vector3> Points)
    {
        float FinalRotation = 0;

        //Get the final point on the spline and check in which direction it is
        Vector3 FinalPoint = Points[Points.Count - 1];
        int SplineDirection = DirectionCheck(Points);
        
        //If the direction is 0, meaning straight track, skip all the points in between the first and last and go straight towards the last (much more efficient)
        if (SplineDirection == 0)
        {
            if (RequestedSpeed < StockInfo.maxSpeed && Direction)
            {
                RequestedSpeed += 10;
            }
            else if (RequestedSpeed > -StockInfo.maxSpeed && !Direction)
            {
                RequestedSpeed -= 10;
            }

            while (transform.position != FinalPoint)
            {
                transform.position = Vector3.MoveTowards(transform.position, FinalPoint, Speed * 0.1f * Time.deltaTime);
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
                float RotateTarget = transform.rotation.eulerAngles.y + (45f * SplineDirection / Points.Count);
                transform.rotation = Quaternion.Euler(0, RotateTarget, 0);

                //Move towards next point
                while (transform.position != Point)
                {
                    transform.position = Vector3.MoveTowards(transform.position, Point, Speed * 0.1f * Time.deltaTime);
                    yield return null;
                }
            }

            //Make sure the train is rotated correctly after turn
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

    private float UpdateSpeed(float TargetSpeed)
    {
        if (TargetSpeed > StockInfo.maxSpeed)
        {
            TargetSpeed = StockInfo.maxSpeed;
        }
        else if (TargetSpeed < -StockInfo.maxSpeed)
        {
            TargetSpeed = -StockInfo.maxSpeed;
        }

        float NewSpeed = Speed;

        //if (ConnectedStock[0] != null && Direction)
        //{
        //    NewSpeed = ConnectedStock[0].GetComponent<TrainController>().Speed + (Vector3.Distance(transform.position, ConnectedStock[0].transform.position) - 1f);
        //}
        //else if (ConnectedStock[1] != null && !Direction)
        //{
        //    NewSpeed = ConnectedStock[1].GetComponent<TrainController>().Speed - (Vector3.Distance(transform.position,ConnectedStock[1].transform.position) - 1f);
        //}
        //else
        //{
        NewSpeed = Speed + (TargetSpeed - Speed) * (StockInfo.trainEffect * 0.01f);
        //}

        //print(NewSpeed);

        if (Direction && NewSpeed < 0)
        {
            NewSpeed = 0;
        }
        else if (!Direction && NewSpeed > 0)
        {
            NewSpeed = 0;
        }

        return NewSpeed;
    }
}