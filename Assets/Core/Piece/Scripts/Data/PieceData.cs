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
        // Kullanımı kaldırıldı fakat dökümantasyonda oldugundan dolayı silinmedi.
        public Vector3 startPosition;  
    
        public PieceData()
        {
            triangles = new List<TriangleData>();
        }
    }
}