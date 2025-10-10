using System.Collections.Generic;
using UnityEngine;

namespace Core.Piece.Scripts.Controller.Interfaces
{
    public interface IPieceFactory
    {
        public void GeneratePiece();
        public int GetGridSize();
        public List<Vector3> GetSnapPoints();
        public List<global::Core.Piece.Scripts.Data.Piece> GetPieces();
    }
}