using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class PieceBuilder : IPieceBuilder
{
    private PieceView _pieceViewPrefab;
    private Transform _pieceViewParent;
    
    private IInstantiator _instantiator;
    private IPieceSpawnPositionController _pieceSpawnPositionController;
    private IPieceFactory _pieceFactory;
    private IPieceSaverService _pieceSaverService;
    
    [Inject]
    private void Construct(IInstantiator instantiator, IPieceSpawnPositionController pieceSpawnPositionService, IPieceFactory pieceFactory,
        PieceView pieceViewPrefab, Transform pieceViewParent, IPieceSaverService ıPieceSaverService)
    {
        _instantiator = instantiator;    
        _pieceSpawnPositionController = pieceSpawnPositionService;
        _pieceFactory = pieceFactory;
        _pieceSaverService = ıPieceSaverService;

        _pieceViewPrefab = pieceViewPrefab;
        _pieceViewParent = pieceViewParent;
    }
    
    public void GenerateNewPiece()
    {
        _pieceFactory.GeneratePiece();
        DrawPieces();
    }
    
    private void DrawPieces()
    {
        List<PieceView> spawnedPieces = new List<PieceView>();;
        List<Piece> pieces = _pieceFactory.GetPieces();
        List<Vector3> snapPoints = _pieceFactory.GetSnapPoints();
        
        for (int i = 0; i < pieces.Count; i++)
        {
            Piece piece = pieces[i];
            Mesh mesh = piece.CreateMesh();
            
            var pieceGo = _instantiator.InstantiatePrefabForComponent<PieceView>(_pieceViewPrefab, _pieceViewParent);
            pieceGo.name = "Piece_" + piece.ID;
            mesh.name = "Piece_" + piece.ID + "_Mesh";
            
            pieceGo.SetPiece(piece.Triangles,snapPoints);
            pieceGo.SetMesh(mesh);
            
            pieceGo.transform.position = _pieceSpawnPositionController.GetSpawnPosition();
            spawnedPieces.Add(pieceGo);
        }
        
        _pieceSaverService.SavePieceData(spawnedPieces, pieces,snapPoints);
    }

    public void Clear()
    {
        _pieceSaverService.Clear();
    }

    // void OnDrawGizmos()
    // {
    //     if (_snapPoints == null || _snapPoints.Count == 0) return;
    //     
    //     Gizmos.color = Color.red;
    //     foreach (var point in _snapPoints)
    //     {
    //         Gizmos.DrawWireSphere(point, 0.15f);
    //     }
    // }
}