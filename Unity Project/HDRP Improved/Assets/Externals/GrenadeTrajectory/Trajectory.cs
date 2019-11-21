using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public Transform endTransform;
    public Vector3 startVel;
    public float duration;
    private float timeCounter;
    public float drawTime;

    public LineRenderer lineRenderer;

    private int currentPoint;
    public int maxPoints;

    private Vector3 startPos;
    private Vector3 previousPos;

    [SerializeField] bool drawing;

    public List<Vector3> points;

    public void Start()
    {
        timeCounter = 0;
        drawing = false;        
        points = new List<Vector3>();
    }


    public void Update()
    {
        if(!drawing)
        {
            timeCounter = 0;
            currentPoint = 0;
            startPos = this.transform.position;
            previousPos = startPos;
            drawing = true;
            points.Clear();
            StartCoroutine(DrawTrajectory());
        }
    }

    public IEnumerator DrawTrajectory() //mantener el WHILE en corrutina por si peta, asi peta una threat aparte.
    {
        while(timeCounter < duration)
        {
            PlotTrajectory(startPos, startVel, timeCounter, duration);
            timeCounter += duration / maxPoints;
        }        

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
        endTransform.position = points[points.Count - 1];

        yield return new WaitForSeconds(drawTime);

        drawing = false;
    }

    public Vector3 PlotTrajectoryAtTime(Vector3 start, Vector3 startVelocity, float time)
    {
        return start + startVelocity * time + Physics.gravity * time * time * 0.5f;
    }

    public void PlotTrajectory(Vector3 start, Vector3 startVelocity, float timestep, float maxTime)
    {
        Vector3 currentPos = PlotTrajectoryAtTime(start, startVelocity, timestep);
        currentPoint++;

        if (currentPoint >= maxPoints)
        {
            timeCounter = duration;
            return;
        }

        RaycastHit hit = new RaycastHit();
        if(Physics.Linecast(previousPos, currentPos, out hit))
        {
            currentPos = hit.point;
            timeCounter = duration;
        }        
        points.Add(currentPos);

        previousPos = currentPos;
    }
}
