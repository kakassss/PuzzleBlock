using System.Collections.Generic;
using Core.Piece.Scripts.View;
using UnityEngine;

namespace Core.Piece.Scripts.Controller
{
    public interface IPieceDrawer
    {
        public List<PieceView> DrawPieces(List<Data.Piece> pieces, List<Vector3> snapPoints,PieceView pieceViewPrefab, Transform parent);
    }
}