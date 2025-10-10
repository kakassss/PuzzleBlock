using UnityEngine;

namespace Core.Piece.Scripts.Controller.Interfaces
{
    public interface IPieceZOrderController
    {
        public void BringToFront(Transform transform);
    }
}