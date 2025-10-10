using System.Collections.Generic;
using Core.Piece.Scripts.Controller.Interfaces;
using Core.Piece.Scripts.View;
using UnityEngine;

namespace Core.Piece.Scripts.Controller
{
    public class PieceSaverService : IPieceSaverService
    {
        private List<PieceView> _spawnedPieces = new List<PieceView>();
        private List<global::Core.Piece.Scripts.Data.Piece> _pieces = new List<global::Core.Piece.Scripts.Data.Piece>();
        private List<Vector3> _snapPoints = new List<Vector3>();
    
        public void SavePieceData(List<PieceView> spawnedPieces, List<global::Core.Piece.Scripts.Data.Piece> pieces,List<Vector3> snapPoints)
        {
            _spawnedPieces = spawnedPieces;
            _pieces = pieces;
            _snapPoints = snapPoints;
        }
    
        public List<PieceView> GetSpawnedPieces() => _spawnedPieces;
        public List<global::Core.Piece.Scripts.Data.Piece> GetPieces() => _pieces;
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
}