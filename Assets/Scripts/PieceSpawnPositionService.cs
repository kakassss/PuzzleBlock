using UnityEngine;

public class PieceSpawnPositionService : IPieceSpawnPositionService
{
    private IGridController _gridController;

    public PieceSpawnPositionService(IGridController gridController)
    {
        _gridController = gridController;
    }
    
    public Vector3 GetSpawnPosition()
    {
        var gridSize = _gridController.GetGridSize();
        
        float randomX = Random.Range(gridSize - gridSize, gridSize);
        float randomY = Random.Range(-gridSize + 1, gridSize);
        
        return new Vector3(randomX, randomY, 0);
    }
}