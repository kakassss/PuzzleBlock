using System.Collections.Generic;
using UnityEngine;
using Zenject;
// ZOrderService.cs
public interface IZOrderService
{
    float BringToFront();
    void Reset();
}

public class ZOrderService : IZOrderService
{
    private float _currentZOrder = 0f;
    private const float Z_DEPTH_INCREMENT = 0.01f;

    public float BringToFront()
    {
        _currentZOrder -= Z_DEPTH_INCREMENT;
        return _currentZOrder;
    }

    public void Reset() => _currentZOrder = 0f;
}

// PieceService.cs
public interface IPieceService
{
    List<Vector3> GetSnapPoints();
    Vector3 GetMouseWorldPosition();
}


public class PieceController
{
    private readonly PieceView _view;
    private readonly IGridController _gridController;
    private readonly IZOrderService _zOrderService;
    private readonly IPieceService _pieceService;
    
    private List<TriangleCell> _triangles = new List<TriangleCell>();
    private Dictionary<TriangleCell, GridCell> _triangleToCellMap = new Dictionary<TriangleCell, GridCell>();
    private Vector3 _pieceCenter;
    private Vector3 _dragOffset;
    private Vector3 _originalPosition;
    private bool _isDragging = false;
    
    [Inject]
    public PieceController(
        PieceView view,
        IGridController gridController,
        IZOrderService zOrderService,
        IPieceService pieceService)
    {
        _view = view;
        _gridController = gridController;
        _zOrderService = zOrderService;
        _pieceService = pieceService;
        
        _view.Initialize(this);
    }

    public void InitializePiece(List<TriangleCell> triangles, Mesh mesh)
    {
        _triangles = triangles;
        Color randomColor = Random.ColorHSV();
        _view.SetVisual(mesh, randomColor);
        CalculatePieceCenter();
    }

    public void HandleMouseDown()
    {
        _isDragging = true;
        _originalPosition = _view.GetWorldPosition();
        
        if (_triangleToCellMap.Count > 0)
        {
            ClearFromGrid();
        }
        
        BringToFront();
        CalculatePieceCenter();
        CacheDragOffset();
    }

    public void HandleMouseDrag()
    {
        if (!_isDragging) return;
        
        Vector3 mousePosition = GetMouseWorldPosition();
        _view.UpdatePosition(mousePosition + _dragOffset);
    }

    public void HandleMouseUp()
    {
        if (!_isDragging) return;
        _isDragging = false;
        
        SnapResult snapResult = FindBestSnapPosition();
        
        if (snapResult.IsValid)
        {
            _view.UpdatePosition(snapResult.Position);
            
            if (!TryPlaceOnGrid())
            {
                _view.UpdatePosition(_originalPosition);
            }
        }
        else
        {
            _view.UpdatePosition(_originalPosition);
        }
    }

    private void CalculatePieceCenter()
    {
        if (_triangles.Count == 0) return;
        
        Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
    
        foreach (var triangle in _triangles)
        {
            foreach (var vertex in triangle.Vertices)
            {
                min = Vector3.Min(min, vertex);
                max = Vector3.Max(max, vertex);
            }
        }
        
        _pieceCenter = (min + max) / 2f;
    }

    private SnapResult FindBestSnapPosition()
    {
        var snapPoints = _pieceService.GetSnapPoints();
        
        if (snapPoints == null || snapPoints.Count == 0) 
            return SnapResult.Invalid;
    
        float bestDistance = float.MaxValue;
        Vector3 bestPosition = Vector3.zero;
        bool foundValidSnap = false;
        const float SNAP_THRESHOLD = 1.0f;
    
        foreach (var triangle in _triangles)
        {
            Vector3 cornerWorldPos = GetTriangleCornerWorldPosition(triangle);
            
            foreach (var snapPoint in snapPoints)
            {
                float distance = CalculateHorizontalDistance(cornerWorldPos, snapPoint);
            
                if (distance < SNAP_THRESHOLD && distance < bestDistance)
                {
                    bestDistance = distance;
                    bestPosition = CalculateNewPiecePosition(cornerWorldPos, snapPoint);
                    foundValidSnap = true;
                }
            }
        }
    
        return foundValidSnap ? new SnapResult(bestPosition, true) : SnapResult.Invalid;
    }

    private Vector3 GetTriangleCornerWorldPosition(TriangleCell triangle)
    {
        Vector3 cornerVertex = triangle.Vertices[0];
        Vector3 normalizedCorner = cornerVertex - _pieceCenter;
        return _view.transform.TransformPoint(normalizedCorner);
    }

    private float CalculateHorizontalDistance(Vector3 worldPos, Vector3 snapPoint)
    {
        Vector2 pos2D = new Vector2(worldPos.x, worldPos.y);
        Vector2 snap2D = new Vector2(snapPoint.x, snapPoint.y);
        return Vector2.Distance(pos2D, snap2D);
    }

    private Vector3 CalculateNewPiecePosition(Vector3 cornerWorldPos, Vector3 snapPoint)
    {
        Vector3 offset = cornerWorldPos - _view.GetWorldPosition();
        Vector3 newPos = snapPoint - offset;
        newPos.z = _view.GetWorldPosition().z;
        return newPos;
    }

    private bool TryPlaceOnGrid()
    {
        List<(TriangleCell triangle, GridCell gridCell)> placements = 
            new List<(TriangleCell, GridCell)>();
    
        foreach (var triangle in _triangles)
        {
            Vector3 worldCenter = GetTriangleWorldCenter(triangle);
            GridCell gridCell = _gridController.GetCellAtPosition(worldCenter);
        
            if (gridCell == null || !gridCell.CanPlaceTriangle(triangle))
                return false;
        
            placements.Add((triangle, gridCell));
        }

        _triangleToCellMap.Clear();
        
        foreach (var (triangle, gridCell) in placements)
        {
            gridCell.PlaceTriangle(triangle);
            _triangleToCellMap[triangle] = gridCell;
        }
    
        return true;
    }

    private Vector3 GetTriangleWorldCenter(TriangleCell triangle)
    {
        Vector3 localCenter = Vector3.zero;
        foreach (var vertex in triangle.Vertices)
            localCenter += vertex;
        localCenter /= triangle.Vertices.Length;
    
        Vector3 normalizedCenter = localCenter - _pieceCenter;
        return _view.transform.TransformPoint(normalizedCenter);
    }
    
    private void ClearFromGrid()
    {
        foreach (var kvp in _triangleToCellMap)
        {
            TriangleCell triangle = kvp.Key;
            GridCell gridCell = kvp.Value;
            
            if (gridCell.HasTriangle(triangle))
                gridCell.RemoveTriangle(triangle);
        }
        
        _triangleToCellMap.Clear();
    }

    private void CacheDragOffset()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        _dragOffset = _view.GetWorldPosition() - mousePosition;
    }

    private Vector3 GetMouseWorldPosition()
    {
        return _pieceService.GetMouseWorldPosition();
    }
    
    private void BringToFront()
    {
        float newZOrder = _zOrderService.BringToFront();
        Vector3 position = _view.GetWorldPosition();
        position.z = newZOrder;
        _view.UpdatePosition(position);
    }
}