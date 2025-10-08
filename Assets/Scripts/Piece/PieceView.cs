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
 
    private IPieceZOrderService _pieceZOrderService;
    private IPieceCenterService _pieceCenterService;
    private IPieceMouseInputHandler _pieceMouseInputHandler;
    private IPiecePlacementService _piecePlacementService;
    private IPieceSnapService _pieceSnapService;
    private DiContainer _diContainer;
    
    [Inject]
    private void Construct(DiContainer diContainer)
    {
        _diContainer = diContainer;
        
        Resolve();
    }

    private void Resolve()
    {
        _pieceZOrderService = _diContainer.Resolve<IPieceZOrderService>();
        _pieceMouseInputHandler = _diContainer.Resolve<IPieceMouseInputHandler>();
        
        _pieceCenterService = _diContainer.Instantiate<PieceCenterService>();
        _piecePlacementService = _diContainer.Instantiate<PiecePlacementService>();
        _pieceSnapService = _diContainer.Instantiate<PieceSnapService>();
    }
    
    private void OnDestroy()
    {
        _piecePlacementService.ClearFromGrid();
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
        Vector3 centerPosition = _pieceCenterService.GetPieceCenter();
        _pieceSnapService.Initialize(_allTriangles,_snapPoints,centerPosition);
        _piecePlacementService.Initialize(_allTriangles,centerPosition);
    }
    
    private void CalculatePieceCenter()
    {
        _pieceCenterService.CalculatePieceCenter(_allTriangles);
    }
    
    private void OnMouseDown()
    {
        _isDragging = true;
        _piecePlacementService.ClearFromGrid();
        
        CalculatePieceCenter();
        _pieceZOrderService.BringToFront(transform);
        
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
            
            if (!_piecePlacementService.TryPlaceOnGrid(transform))
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
            Vector3 normalizedCorner = cornerVertex - _pieceCenterService.GetPieceCenter();
            Vector3 worldCorner = transform.TransformPoint(normalizedCorner);
            
            Gizmos.DrawWireSphere(worldCorner, 0.1f);
        }
    }
}