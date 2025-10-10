using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PieceLoader : IPieceLoader
{
    private List<TriangleCell> _allTriangles = new List<TriangleCell>();
    private List<Piece> _pieces = new List<Piece>();
    private List<Vector3> _snapPoints = new List<Vector3>();
    private List<PieceView> _spawnedPieces = new List<PieceView>();

    private Transform _parentTransform;
    private PieceView _pieceViewPrefab;
    
    private ITriangleNeighborService _triangleNeighborService;
    private IPieceSpawnPositionController _pieceSpawnPositionController;
    private ILevelCompletionController _levelCompletionController;
    private IInstantiator _instantiator;

    public PieceLoader(IInstantiator instantiator,IPieceSpawnPositionController pieceSpawnPositionController,
        PieceView pieceViewPrefab,Transform parentTransform,ITriangleNeighborService triangleNeighborService, ILevelCompletionController levelCompletionController)
    {
        _triangleNeighborService = triangleNeighborService;
        _instantiator = instantiator;
        _pieceSpawnPositionController = pieceSpawnPositionController;
        
        _parentTransform = parentTransform;
        _pieceViewPrefab = pieceViewPrefab;
        
        _levelCompletionController = levelCompletionController;
    }
    
    public void LoadFromLevelData(LevelData levelData)
    {
        _snapPoints = new List<Vector3>(levelData.SnapPoints);
        
        foreach (var pieceData in levelData.Pieces)
        {
            Piece piece = new Piece(pieceData.pieceId);
            
            foreach (var triData in pieceData.triangles)
            {
                TriangleCell triangle = new TriangleCell(triData.vertices, false, triData.cell);
                piece.AddTriangle(triangle);
                _allTriangles.Add(triangle);
            }
            
            _pieces.Add(piece);
        }
        
        _triangleNeighborService.FindNeighbors(_allTriangles);
        DrawLoadedPieces(levelData);
    }
    
    private void DrawLoadedPieces(LevelData levelData)
    {
        for (int i = 0; i < _pieces.Count; i++)
        {
            Piece piece = _pieces[i];
            Mesh mesh = piece.CreateMesh();
            
            var pieceGo = _instantiator.InstantiatePrefabForComponent<PieceView>(_pieceViewPrefab, _parentTransform);
            pieceGo.name = "Piece_" + piece.ID;
            mesh.name = "Piece_" + piece.ID + "_Mesh";
            
            pieceGo.SetPiece(piece.Triangles,_snapPoints);
            pieceGo.SetMesh(mesh);

            PieceData pieceData = levelData.Pieces.Find(p => p.pieceId == piece.ID);
            // if (pieceData != null)
            // {
            //     pieceGo.transform.position = pieceData.startPosition;
            // }
            // else
            // {
            pieceGo.transform.position = _pieceSpawnPositionController.GetSpawnPosition();
            _pieceSpawnPositionController.PieceMovementTween(pieceGo.transform);
            //}
            
            _spawnedPieces.Add(pieceGo);
        }
        
        _levelCompletionController.SetLevelTarget(_spawnedPieces.Count);
    }
    
    public void Clear()
    {
        if (_spawnedPieces is { Count: > 0 })
        {
            foreach (var piece in _spawnedPieces)
            {
                Object.Destroy(piece.gameObject);
            }    
        }
        
        _allTriangles.Clear();
        _pieces.Clear();
        _spawnedPieces.Clear();
    }
}