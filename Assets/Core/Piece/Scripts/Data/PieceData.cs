using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Piece.Scripts.Data
{
    [Serializable]
    public class PieceData
    {
        public int pieceId;
        public List<TriangleData> triangles;
        public Vector3 startPosition;
    
        public PieceData()
        {
            triangles = new List<TriangleData>();
        }
    }
}