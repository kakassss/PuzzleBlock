using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PieceView : MonoBehaviour
{
    private static float globalZOrder = 0;
    
    private List<TriangleCell> _allTriangles = new List<TriangleCell>();
    
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _material;
    [SerializeField] private MeshCollider _meshCollider;
    
    private Camera _mainCamera;
    private bool _isDragging = false;
    private Vector3 _dragOffset;
    private Vector3 _originalPosition;
    
    private IGridController _gridController;
    private DiContainer _diContainer;
    private Dictionary<TriangleCell, GridCell> _triangleToCell = new Dictionary<TriangleCell, GridCell>();
    private List<Vector3> _snapPoints;
    private Vector3 _pieceCenter;

    private PieceController _pieceController;
    private Material _instanceMaterial;

    public void Initialize(PieceController controller)
    {
        _pieceController = controller;
    }
    
    [Inject]
    private void Construct(DiContainer diContainer,IGridController gridController)
    {
        _diContainer = diContainer;
        
        _gridController = _diContainer.Resolve<IGridController>();
    }

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void SetTriangles(List<TriangleCell> alltriangles)
    {
        _allTriangles = alltriangles;
    }

    public void SetPiece(Mesh mesh)
    {
        _meshFilter.mesh = mesh;
        Material mat = new Material(_material);
        mat.color = Random.ColorHSV();
        _meshRenderer.material = mat;
        _meshCollider.sharedMesh = mesh;
    }
    
    public void SetSnapPoints(List<Vector3> snapPoints)
    {
        _snapPoints = snapPoints;
    }
    
    public void CalculatePieceCenter()
    {
        if (_allTriangles.Count == 0) return;
        
        Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
    
        foreach (var tri in _allTriangles)
        {
            foreach (var vertex in tri.Vertices)
            {
                min = Vector3.Min(min, vertex);
                max = Vector3.Max(max, vertex);
            }
        }
        _pieceCenter = (min + max) / 2f;
    }

    private void OnMouseDown()
    {
        _isDragging = true;
        _originalPosition = transform.position;
        
        if (_triangleToCell.Count > 0)
        {
            ClearFromGrid();
        }
        CalculatePieceCenter();
        BringToFront();
        
        Vector3 mousePos = GetMouseWorldPosition();
        _dragOffset = transform.position - mousePos;
    }
    
    private void OnMouseDrag()
    {
        if (!_isDragging) return;
        
        Vector3 mousePos = GetMouseWorldPosition();
        transform.position = mousePos + _dragOffset;
    }

    private void OnMouseUp()
    {
        if (!_isDragging) return;
        _isDragging = false;
        
        Vector3? bestSnapPos = FindBestSnapPosition();
    
        if (bestSnapPos.HasValue)
        {
            transform.position = bestSnapPos.Value;
        
            if (!TryPlaceOnGrid())
            {
                transform.position = _originalPosition;
            }
        }
    }

    private Vector3? FindBestSnapPosition()
    {
        if (_snapPoints == null || _snapPoints.Count == 0) return null;
    
        float bestDistance = float.MaxValue;
        Vector3? bestPosition = null;
        float snapThreshold = 1.0f;
    
        foreach (var triangle in _allTriangles)
        {
            Vector3 cornerVertex = GetTriangleCornerVertex(triangle);
            Vector3 normalizedCorner = cornerVertex - _pieceCenter;
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

    private bool TryPlaceOnGrid()
    {
        List<(TriangleCell triangle, GridCell gridCell)> placements = new List<(TriangleCell, GridCell)>();
    
        foreach (var triangle in _allTriangles)
        {
            Vector3 worldPos = GetTriangleWorldCenter(triangle);
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

    private Vector3 GetTriangleWorldCenter(TriangleCell triangle)
    {
        Vector3 localCenter = Vector3.zero;
        foreach (var vertex in triangle.Vertices)
            localCenter += vertex;
        localCenter /= triangle.Vertices.Length;
    
        Vector3 normalizedCenter = localCenter - _pieceCenter;
        return transform.TransformPoint(normalizedCenter);
    }
    
    private void ClearFromGrid()
    {
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

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(_mainCamera.transform.position.z);
        return _mainCamera.ScreenToWorldPoint(mousePos);
    }
    
    private void BringToFront()
    {
        globalZOrder -= 0.01f;
        Vector3 pos = transform.position;
        pos.z = globalZOrder;
        transform.position = pos;
    }
    
    private void OnDrawGizmos()
    {
        if (_allTriangles == null || _allTriangles.Count == 0) return;
        
        Gizmos.color = Color.yellow;
        
        foreach (var triangle in _allTriangles)
        {
            Vector3 cornerVertex = GetTriangleCornerVertex(triangle);
            Vector3 normalizedCorner = cornerVertex - _pieceCenter;
            Vector3 worldCorner = transform.TransformPoint(normalizedCorner);
            
            Gizmos.DrawWireSphere(worldCorner, 0.1f);
        }
    }
}