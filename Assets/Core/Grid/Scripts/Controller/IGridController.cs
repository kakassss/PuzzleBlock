using System.Collections.Generic;
using Core.Grid.Scripts.Data;
using Core.Piece.Scripts.Data;
using UnityEngine;

namespace Core.Grid.Scripts.Controller
{
    public interface IGridController
    {
        public List<TriangleCell> GetAllTriangleCells();
        public Vector3 GetWorldPosition(int x, int y);
        public void SetValue(int x, int y, GridCell value);
        public void GetXY(Vector3 worldPosition, out int x, out int y);
        public void SetValue(Vector3 worldPositon, GridCell value);
        public int GetGridSize();
        public GridCell GetValue(int x, int y);
        public GridCell GetValue(Vector3 worldPositon);
        public List<Vector3> GetGridVisualPositions();
        public List<Vector3> GetCenterPositions();
        public bool InBounds(int x, int y);
    }
}