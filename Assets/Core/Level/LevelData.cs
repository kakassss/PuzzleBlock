using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int GridSize;
    public List<PieceData> Pieces;
    public List<Vector3> SnapPoints;
    public GameDifficultySOData GameDifficulty;
    
    public LevelData()
    {
        Pieces = new List<PieceData>();
        SnapPoints = new List<Vector3>();
    }
}