using System.Collections.Generic;
using Core.Piece.Scripts.Data;
using UnityEngine;

namespace Core.Piece.Scripts.Controller.Interfaces
{
    public interface IPieceCenterController
    {
        public void CalculatePieceCenter(List<TriangleCell> allTriangles);
        public Vector3 GetPieceCenter();
    }
}