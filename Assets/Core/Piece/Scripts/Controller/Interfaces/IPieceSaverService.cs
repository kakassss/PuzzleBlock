using System.Collections.Generic;
using Core.Piece.Scripts.View;
using UnityEngine;

namespace Core.Piece.Scripts.Controller.Interfaces
{
    public interface IPieceSaverService
    {
        public void SavePieceData(List<PieceView> spawnedPieces, List<global::Core.Piece.Scripts.Data.Piece> pieces, List<Vector3> snapPoints);
        public List<PieceView> GetSpawnedPieces();
        public List<global::Core.Piece.Scripts.Data.Piece> GetPieces();
        public List<Vector3> GetSnapPoints();
        public void Clear();
    }
}