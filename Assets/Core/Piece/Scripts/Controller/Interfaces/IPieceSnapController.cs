using System.Collections.Generic;
using Core.Piece.Scripts.Data;
using UnityEngine;

namespace Core.Piece.Scripts.Controller.Interfaces
{
    public interface IPieceSnapController
    {
        public Vector3? FindBestSnapPosition(Transform transform);
        public void Initialize(List<TriangleCell> allTriangles, List<Vector3> snapPoints, Vector3 centerPoint);
    }
}