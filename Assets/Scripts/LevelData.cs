using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int gridSize;
    public List<PieceData> pieces;
    public List<Vector3> snapPoints;
    
    public LevelData()
    {
        pieces = new List<PieceData>();
        snapPoints = new List<Vector3>();
    }
}