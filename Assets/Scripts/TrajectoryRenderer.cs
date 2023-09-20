using System;
using UnityEditor;
using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    [SerializeField] private int length = 20;
    private LineRenderer _lineRendererComponent;

    private void Start()
    {
        _lineRendererComponent = GetComponent<LineRenderer>();
    }

    public void ToggleOnTrajectory() => _lineRendererComponent.enabled = true;
    public void ToggleOffTrajectory() => _lineRendererComponent.enabled = false;

    public void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[length];
        _lineRendererComponent.positionCount = points.Length;

        for (int i = 0; i < points.Length; i++)
        {
            int index = i;
            float time = i * .1f;
            points[i] = origin + speed * time + Physics.gravity * (time * time) / 2f;
            _lineRendererComponent.SetPosition(index,points[i]);
        }
    }
}

public static class Bezier 
{

    public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, float t) 
    {
        return (1.0f - t) * (1.0f - t) * p0 
               + 2.0f * (1.0f - t) * t * p1 + t * t * p2;;
    }

    public static Vector3[] GetPoints(Vector3 p0, Vector3 p1, Vector3 p2, int count)
    {
        Vector3[] points = new Vector3[count];
        float t =0;

        for(int i = 0; i < points.Length; i++)
        {
            t = i / (points.Length - 1.0f);
            points[i] = GetPoint(p0,p1,p2,t);
        }

        return points;
    }
    public static Vector3 CalcBallisticVelocityVector(Vector3 source, Vector3 target, float angle)
    {
        Vector3 direction = target - source;                            
        float h = direction.y;                                           
        direction.y = 0;                                               
        float distance = direction.magnitude;                           
        float a = angle * Mathf.Deg2Rad;                                
        direction.y = distance * Mathf.Tan(a);                            
        distance += h/Mathf.Tan(a);                                      
 
        // calculate velocity
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2*a));
        return velocity * direction.normalized;    
    }
}
