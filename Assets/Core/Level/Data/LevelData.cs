using System;
using System.Collections.Generic;
using Core.Piece.Scripts.Data;
using UnityEngine;

namespace Core.Level.Data
{
    [Serializable]
    public class LevelData
    {
        public int GridSize;
        public List<PieceData> Pieces;
        public List<Vector3> SnapPoints;
    
        public LevelData()
        {
            Pieces = new List<PieceData>();
            SnapPoints = new List<Vector3>();
        }
    }
}