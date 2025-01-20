using UnityEngine;

public class DottedCurvedLine : MonoBehaviour
{
    /*
        To use this:

        Create a new GameObject in your scene
        Add a LineRenderer component to it
        Add this script to the same GameObject
        Set up the LineRenderer material:

        Create a new material with a dot texture (white circle on transparent background)
        Assign it to the LineRenderer
        
        code:
        
        DottedCurvedLine line = GetComponent<DottedCurvedLine>();
        line.DrawDottedCurve(
            new Vector2(transform.position.x, transform.position.y),           // start point
            new Vector2(target.transform.position.x, target.transform.position.y),           // end point
            new Vector2(2.5f, 2.5f)      // control point
        );
        
     */
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float spacing = 0.1f;  // Space between dots
    [SerializeField] private int resolution = 50;    // How smooth the curve is
    [SerializeField] private float dotSize = 0.1f;   // Size of each dot
    
    public void DrawDottedCurve(Vector2 start, Vector2 end, Vector2 controlPoint)
    {
        // Calculate how many dots we need based on the curve length
        Vector2[] points = CalculateCurvePoints(start, end, controlPoint);
        float curveLength = CalculateCurveLength(points);
        int numberOfDots = Mathf.CeilToInt(curveLength / spacing);
        
        // Set up the line renderer
        lineRenderer.positionCount = numberOfDots;
        lineRenderer.startWidth = dotSize;
        lineRenderer.endWidth = dotSize;
        
        // Create the dotted effect using material
        lineRenderer.material.mainTextureScale = new Vector2(1f / spacing, 1);
        lineRenderer.textureMode = LineTextureMode.Tile;
        
        // Place dots along the curve
        for (int i = 0; i < numberOfDots; i++)
        {
            float t = i / (float)(numberOfDots - 1);
            Vector2 position = CalculateQuadraticBezierPoint(t, start, controlPoint, end);
            lineRenderer.SetPosition(i, new Vector3(position.x, position.y, 0));
        }
    }
    
    private Vector2[] CalculateCurvePoints(Vector2 start, Vector2 end, Vector2 controlPoint)
    {
        Vector2[] points = new Vector2[resolution];
        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            points[i] = CalculateQuadraticBezierPoint(t, start, controlPoint, end);
        }
        return points;
    }
    
    private Vector2 CalculateQuadraticBezierPoint(float t, Vector2 start, Vector2 control, Vector2 end)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        
        Vector2 point = uu * start;
        point += 2 * u * t * control;
        point += tt * end;
        
        return point;
    }
    
    private float CalculateCurveLength(Vector2[] points)
    {
        float length = 0;
        for (int i = 0; i < points.Length - 1; i++)
        {
            length += Vector2.Distance(points[i], points[i + 1]);
        }
        return length;
    }
}