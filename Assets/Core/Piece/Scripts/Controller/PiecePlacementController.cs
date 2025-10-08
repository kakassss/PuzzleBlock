using System.Collections.Generic;
using UnityEngine;

public class PiecePlacementController : IPiecePlacementController
{
    private List<TriangleCell> _allTriangles = new List<TriangleCell>();
    private Dictionary<TriangleCell, GridCell> _triangleToCell = new Dictionary<TriangleCell, GridCell>();
    private Vector3 _centerPosition;
    
    private IGridController _gridController;

    public PiecePlacementController(IGridController gridController)
    {
        _gridController = gridController;
    }

    public void Initialize(List<TriangleCell> allTriangles, Vector3 centerPosition)
    {
        _allTriangles = allTriangles;
        _centerPosition = centerPosition;
    }
    
    public bool TryPlaceOnGrid(Transform transform)
    {
        List<(TriangleCell triangle, GridCell gridCell)> placements = new List<(TriangleCell, GridCell)>();
    
        foreach (var triangle in _allTriangles)
        {
            Vector3 worldPos = GetTriangleWorldCenter(transform,triangle);
            var gridCell = _gridController.GetValue(worldPos);
        
            if (gridCell == null)
            {
                return false;
            }
        
            if (!gridCell.CanPlace(triangle))
            {
                return false;
            }
        
            placements.Add((triangle, gridCell));
        }

        _triangleToCell.Clear();
    
        foreach (var (triangle, gridCell) in placements)
        {
            gridCell.AddTriangle(triangle);
            _triangleToCell[triangle] = gridCell;
        }
    
        return true;
    }
    
    public void ClearFromGrid()
    {
        if(_triangleToCell.Count == 0) return;
        
        foreach (var kvp in _triangleToCell)
        {
            TriangleCell triangle = kvp.Key;
            GridCell gridCell = kvp.Value;
            
            if (gridCell.HasTriangle(triangle))
            {
                gridCell.RemoveTriangle(triangle);
            }

        }
        
        _triangleToCell.Clear();
    }
    
    private Vector3 GetTriangleWorldCenter(Transform transform,TriangleCell triangle)
    {
        Vector3 localCenter = Vector3.zero;
        foreach (var vertex in triangle.Vertices)
            localCenter += vertex;
        localCenter /= triangle.Vertices.Length;
    
        Vector3 normalizedCenter = localCenter - _centerPosition;
        return transform.TransformPoint(normalizedCenter);
    }
}
