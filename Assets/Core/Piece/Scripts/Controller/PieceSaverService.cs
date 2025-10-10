using System.Collections.Generic;
using UnityEngine;

public class PieceSaverService : IPieceSaverService
{
    private List<PieceView> _spawnedPieces = new List<PieceView>();
    private List<Piece> _pieces = new List<Piece>();
    private List<Vector3> _snapPoints = new List<Vector3>();
    
    public void SavePieceData(List<PieceView> spawnedPieces, List<Piece> pieces,List<Vector3> snapPoints)
    {
        _spawnedPieces = spawnedPieces;
        _pieces = pieces;
        _snapPoints = snapPoints;
    }
    
    public List<PieceView> GetSpawnedPieces() => _spawnedPieces;
    public List<Piece> GetPieces() => _pieces;
    public List<Vector3> GetSnapPoints() => _snapPoints;

    public void Clear()
    {
        if (_spawnedPieces is { Count: > 0 })
        {
            foreach (var piece in _spawnedPieces)
            {
                Object.Destroy(piece.gameObject);
            }    
        }
        
        
        _spawnedPieces.Clear();
        _pieces.Clear();
        _snapPoints.Clear();
    }
}