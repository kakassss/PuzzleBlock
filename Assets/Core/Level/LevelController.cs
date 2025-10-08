using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GridSplitter _gridSplitter;
    
    private IPieceSpawnPositionController _pieceSpawnPositionController;
    private ILevelPersistenceService _levelPersistenceService;

    [Inject]
    private void Construct(IPieceSpawnPositionController pieceSpawnPositionService, ILevelPersistenceService levelPersistenceService)
    {
        _pieceSpawnPositionController = pieceSpawnPositionService;
        _levelPersistenceService = levelPersistenceService;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveLevel("Level_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss"));
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            string lastLevel = _levelPersistenceService.GetLastPlayedLevel();
            
            if (!string.IsNullOrEmpty(lastLevel))
            {
                LoadLevel(lastLevel);
            }
            else
            {
                Debug.LogWarning("No previously saved level found!");
            }
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            GenerateNewLevel();
        }
    }
    
    private void SaveLevel(string levelName)
    {
        LevelData levelData = new LevelData
        {
            gridSize = _gridSplitter.GetGridSize(),
            snapPoints = new List<Vector3>(_gridSplitter.GetSnapPoints())
        };
        
        var pieces = _gridSplitter.GetPieces();
        var spawnedPieces = _gridSplitter.GetSpawnedPieces();
        
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
            
            levelData.pieces.Add(pieceData);
        }
        
        _levelPersistenceService.SaveLevel(levelData, levelName);
        _levelPersistenceService.SaveLastPlayedLevel(levelName);
    }
    
    private void LoadLevel(string levelName)
    {
        LevelData levelData = _levelPersistenceService.LoadLevel(levelName);
        
        if (levelData == null)
        {
            Debug.LogError($"Failed to load level: {levelName}");
            return;
        }
        
        _gridSplitter.ClearLevel();
        _gridSplitter.LoadFromLevelData(levelData);
        
        _levelPersistenceService.SaveLastPlayedLevel(levelName);
    }
    
    private void GenerateNewLevel()
    {
        _gridSplitter.ClearLevel();
        _gridSplitter.GenerateNewLevel();
    }
}
