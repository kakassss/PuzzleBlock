using System.Collections.Generic;
using UnityEngine;

public class PieceCenterService : IPieceCenterService
{
    private Vector3 _pieceCenter;
    
    public void CalculatePieceCenter(List<TriangleCell> allTriangles)
    {
        if (allTriangles.Count == 0) return;
        
        Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
    
        foreach (var tri in allTriangles)
        {
            foreach (var vertex in tri.Vertices)
            {
                min = Vector3.Min(min, vertex);
                max = Vector3.Max(max, vertex);
            }
        }
        _pieceCenter = (min + max) / 2f;
    }

    public Vector3 GetPieceCenter()
    {
        return _pieceCenter;
    }
}