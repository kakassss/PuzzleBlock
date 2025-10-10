using System.Collections.Generic;
using Core.Piece.Scripts.Data;
using UnityEngine;

namespace Core.Piece.Scripts.Controller.Interfaces
{
    public interface IPiecePlacementController
    {
        public void Initialize(List<TriangleCell> allTriangles, Vector3 centerPosition);
        public bool TryPlaceOnGrid(Transform transform);
        public void ClearFromGrid();
    }
}