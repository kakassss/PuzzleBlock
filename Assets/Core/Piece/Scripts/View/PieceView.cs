using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class PieceView : MonoBehaviour
{
    private List<TriangleCell> _allTriangles = new List<TriangleCell>();
    
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _material;
    [SerializeField] private MeshCollider _meshCollider;
    
    private Material _instanceMaterial;
    private bool _isDragging = false;
    private List<Vector3> _snapPoints;
 
    private IPieceZOrderController _ıPieceZOrderController;
    private IPieceCenterController _pieceCenterController;
    private IPieceMouseInputHandler _pieceMouseInputHandler;
    private IPiecePlacementController _piecePlacementController;
    private IPieceSnapController _pieceSnapService;
    private DiContainer _diContainer;
    
    [Inject]
    private void Construct(DiContainer diContainer, IPieceMouseInputHandler pieceMouseInputHandler, IPieceZOrderController ıPieceZOrderController)
    {
        _diContainer = diContainer;
        _pieceMouseInputHandler = pieceMouseInputHandler;
        _ıPieceZOrderController = ıPieceZOrderController;
        
        Instantiate();
    }

    private void Instantiate()
    {
        _pieceCenterController = _diContainer.Instantiate<PieceCenterController>();
        _piecePlacementController = _diContainer.Instantiate<PiecePlacementController>();
        _pieceSnapService = _diContainer.Instantiate<PieceSnapController>();
    }
    
    private void OnDestroy()
    {
        _piecePlacementController.ClearFromGrid();
    }
    
    public void SetPiece(List<TriangleCell> alltriangles,List<Vector3> snapPoints)
    {
        _allTriangles = alltriangles;
        _snapPoints = snapPoints;
        CalculatePieceCenter();
        
        Initialize();
    }

    public void SetMesh(Mesh mesh)
    {
        Material mat = new Material(_material);
        mat.color = Random.ColorHSV();
        
        _meshFilter.mesh = mesh;
        _meshRenderer.material = mat;
        _meshCollider.sharedMesh = mesh;
    }
    
    private void Initialize()
    {
        Vector3 centerPosition = _pieceCenterController.GetPieceCenter();
        _pieceSnapService.Initialize(_allTriangles,_snapPoints,centerPosition);
        _piecePlacementController.Initialize(_allTriangles,centerPosition);
    }
    
    private void CalculatePieceCenter()
    {
        _pieceCenterController.CalculatePieceCenter(_allTriangles);
    }
    
    private void OnMouseDown()
    {
        _isDragging = true;
        _piecePlacementController.ClearFromGrid();
        
        CalculatePieceCenter();
        _ıPieceZOrderController.BringToFront(transform);
        
        _pieceMouseInputHandler.HandleMouseDown(transform);
    }
    
    private void OnMouseDrag()
    {
        if (!_isDragging) return;
        
        _pieceMouseInputHandler.HandleMouseDrag(transform);
    }

    private void OnMouseUp()
    {
        if (!_isDragging) return;
        _isDragging = false;
        
        Vector3? bestSnapPos = _pieceSnapService.FindBestSnapPosition(transform);
    
        if (bestSnapPos.HasValue)
        {
            transform.position = bestSnapPos.Value;
            
            if (!_piecePlacementController.TryPlaceOnGrid(transform))
            {
                _pieceMouseInputHandler.HandleMouseUp(transform);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        if (_allTriangles == null || _allTriangles.Count == 0) return;
        
        Gizmos.color = Color.yellow;
        
        foreach (var triangle in _allTriangles)
        {
            Vector3 cornerVertex = triangle.Vertices[0];
            Vector3 normalizedCorner = cornerVertex - _pieceCenterController.GetPieceCenter();
            Vector3 worldCorner = transform.TransformPoint(normalizedCorner);
            
            Gizmos.DrawWireSphere(worldCorner, 0.1f);
        }
    }
}