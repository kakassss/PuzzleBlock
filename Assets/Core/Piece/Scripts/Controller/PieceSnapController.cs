using System.Collections.Generic;
using UnityEngine;

public class PieceSnapController : IPieceSnapController
{
    private List<Vector3> _snapPoints;
    private List<TriangleCell> _allTriangles = new List<TriangleCell>();
    private Vector3 _centerPoint;
    
    private IPieceMouseInputHandler _pieceMouseInputHandler;

    public PieceSnapController(IPieceMouseInputHandler pieceMouseInputHandler)
    {
        _pieceMouseInputHandler = pieceMouseInputHandler;
    }
    
    public void Initialize(List<TriangleCell> allTriangles,  List<Vector3> snapPoints, Vector3 centerPoint)
    {
        _allTriangles = allTriangles;
        _snapPoints = snapPoints;
        _centerPoint = centerPoint;
    }
    
    public Vector3? FindBestSnapPosition(Transform transform)
    {
        if (_snapPoints == null || _snapPoints.Count == 0) return null;
        
        float bestDistance = float.MaxValue;
        Vector3? bestPosition = null;
        float snapThreshold = 0.5f;

        if (_pieceMouseInputHandler.MouseWorldPosInGrid() == false) return bestPosition;
        
        foreach (var triangle in _allTriangles)
        {
            Vector3 cornerVertex = GetTriangleCornerVertex(triangle);
            Vector3 normalizedCorner = cornerVertex - _centerPoint;
            Vector3 cornerWorldPos = transform.TransformPoint(normalizedCorner);
        
            foreach (var snapPoint in _snapPoints)
            {
                Vector2 headPos2D = new Vector2(cornerWorldPos.x, cornerWorldPos.y);
                Vector2 snapPos2D = new Vector2(snapPoint.x, snapPoint.y);
                float distance = Vector2.Distance(headPos2D, snapPos2D);
            
                if (distance < snapThreshold && distance < bestDistance)
                {
                    bestDistance = distance;
                    Vector3 offset = cornerWorldPos - transform.position;
                
                    Vector3 newPos = snapPoint - offset;
                    newPos.z = transform.position.z;
                    bestPosition = newPos;
                }
            }
        }
    
        return bestPosition;
    }
    
    private Vector3 GetTriangleCornerVertex(TriangleCell triangle)
    {
        return triangle.Vertices[0];
    }
}
