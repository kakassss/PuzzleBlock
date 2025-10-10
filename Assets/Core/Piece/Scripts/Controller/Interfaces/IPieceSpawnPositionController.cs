using UnityEngine;

public interface IPieceSpawnPositionController
{
    public Vector3 GetSpawnPosition();
    public void PieceMovementTween(Transform transform);
}