using System.Collections.Generic;
using UnityEngine;


public class LevelController : ILevelController
{
    private string _saveLevelName = "Level_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
    
    private readonly IPieceSpawnPositionController _pieceSpawnPositionController;
    private readonly ILevelPersistenceService _levelPersistenceService;
    private readonly IGridController _gridController;
    private readonly IPieceLoader _pieceLoader;
    private readonly IPieceBuilder _pieceBuilder;
    private readonly IPieceSaverService _pieceSaverService;
    
    public LevelController(IPieceSpawnPositionController pieceSpawnPositionService, ILevelPersistenceService levelPersistenceService,
        IGridController gridController, IPieceLoader pieceLoader, IPieceBuilder pieceBuilder,
        IPieceSaverService pieceSaverService)
    {
        _pieceSpawnPositionController = pieceSpawnPositionService;
        _levelPersistenceService = levelPersistenceService;
        _gridController = gridController;
        _pieceLoader = pieceLoader;
        _pieceBuilder = pieceBuilder;
        _pieceSaverService = pieceSaverService;
    }
    
    public void GenerateNewLevel()
    {
        LevelClear();
        _pieceBuilder.GenerateNewPiece();
    }
    
    public void SaveLevel()
    {
        LevelData levelData = new LevelData
        {
            GridSize = _gridController.GetGridSize(),
            SnapPoints = new List<Vector3>(_pieceSaverService.GetSnapPoints())
        };
        
        var pieces = _pieceSaverService.GetPieces();
        var spawnedPieces = _pieceSaverService.GetSpawnedPieces();
        
        foreach (var piece in pieces)
        {
            PieceData pieceData = new PieceData
            {
                pieceId = piece.ID
            };
            
            foreach (var triangle in piece.Triangles)
            {
                TriangleData triData = new TriangleData(triangle.Vertices, triangle.Cell);
                pieceData.triangles.Add(triData);
            }
            
            foreach (var spawnedPiece in spawnedPieces)
            {
                if (spawnedPiece.name == "Piece_" + piece.ID)
                {
                    pieceData.startPosition = _pieceSpawnPositionController.GetSpawnPosition();
                    break;
                }
            }
            
            levelData.Pieces.Add(pieceData);
        }
        
        _levelPersistenceService.SaveLevel(levelData, _saveLevelName);
        _levelPersistenceService.SaveLastPlayedLevel(_saveLevelName);
    }
    
    public void LoadLevel()
    {
        string lastLevel = _levelPersistenceService.GetLastPlayedLevel();
        LevelData levelData = _levelPersistenceService.LoadLevel(lastLevel);
        
        if (levelData == null)
        {
            Debug.LogError($"Failed to load level: {lastLevel}");
            return;
        }

        LevelClear();
        _pieceLoader.LoadFromLevelData(levelData);
        _levelPersistenceService.SaveLastPlayedLevel(lastLevel);
    }
    
    private void LevelClear()
    {
        _pieceBuilder.Clear();
        _pieceLoader.Clear();
    }
}
