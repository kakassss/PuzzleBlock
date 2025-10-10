using DG.Tweening;
using UnityEngine;

public class PieceSpawnPositionController : IPieceSpawnPositionController
{
    private readonly IGridController _gridController;

    public PieceSpawnPositionController(IGridController gridController)
    {
        _gridController = gridController;
    }
    
    public Vector3 GetSpawnPosition()
    {
        var gridSize = _gridController.GetGridSize();
        
        float randomX = Random.Range(2, 3);
        float randomY = Random.Range(10, 13);
        
        return new Vector3(randomX, randomY, 0);
    }

    public void PieceMovementTween(Transform transform)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.SetAutoKill(true);
        var targetPos = transform.position + new Vector3(Random.Range(-1f,3f),Random.Range(-14,-16));
        
        sequence.Append(transform.DOMove(targetPos, .5f));
    }
}