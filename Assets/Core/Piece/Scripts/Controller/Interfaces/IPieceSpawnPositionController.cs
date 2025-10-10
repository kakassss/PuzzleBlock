using UnityEngine;

namespace Core.Piece.Scripts.Controller.Interfaces
{
    public interface IPieceSpawnPositionController
    {
        public Vector3 GetSpawnPosition();
        public void PieceMovementTween(Transform transform);
    }
}