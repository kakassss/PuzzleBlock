using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    private List<TriangleCell> _triangles = new List<TriangleCell>();
    
    public GridCell(Vector3 cell)
    {
    }
    
    public bool CanPlace(TriangleCell triangle)
    {
        return _triangles.Count < 4 && !_triangles.Contains(triangle);
    }
    
    public void AddTriangle(TriangleCell triangle)
    {
        if (!_triangles.Contains(triangle))
        {
            _triangles.Add(triangle);
        }
    }
    
    public void RemoveTriangle(TriangleCell triangle)
    {
        _triangles.Remove(triangle);
    }
    
    public bool HasTriangle(TriangleCell triangle)
    {
        return _triangles.Contains(triangle);
    }
}