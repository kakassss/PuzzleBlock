using System.Collections.Generic;
using Core.GameSettings.Scripts.Controller;
using Core.Grid.Scripts.Controller;
using Core.Piece.Scripts.Controller.Interfaces;
using Core.Piece.Scripts.Data;
using UnityEngine;

namespace Core.Piece.Scripts.Controller
{
    public class PieceFactory : IPieceFactory
    {
        public int GetGridSize() => _gridController.GetGridSize();
        public List<Vector3> GetSnapPoints() => _snapPoints;
        public List<Data.Piece> GetPieces() => _pieces;
    
        private List<TriangleCell> _allTriangles = new List<TriangleCell>();
        private List<Data.Piece> _pieces = new List<Data.Piece>();
        private List<Vector3> _snapPoints = new List<Vector3>();
    
        private IGridController _gridController;
        private ITriangleNeighborService _triangleNeighborService;
        private IGameDifficultyController _gameDifficultyController;
    
        private int _pieceCount;
    
        public PieceFactory(IGridController gridController,ITriangleNeighborService triangleNeighborService,
            IGameDifficultyController gameDifficultyController)
        {
            _gridController = gridController;
            _triangleNeighborService = triangleNeighborService;
            _gameDifficultyController = gameDifficultyController;
        
            var gameDifficulty = _gameDifficultyController.GetDifficultyData();
            _pieceCount = gameDifficulty.PieceAmount;
        }

        public void GeneratePiece()
        {
            _allTriangles = _gridController.GetAllTriangleCells();
            CollectSnapPoints();
            FindNeighborTriangles();
            CreatePieces(_pieceCount);
        }
    
        private void CreatePieces(int targetCount)
        {
            int pieceIdCounter = 0;
            _pieces.Clear();
        
            foreach (var tri in _allTriangles)
            {
                tri.Visited = false;
            }
        
            foreach (var tri in _allTriangles)
            {
                if (!tri.Visited)
                {
                    Data.Piece newPiece = new Data.Piece(pieceIdCounter++);
                    GrowRegion(tri, newPiece);
                    _pieces.Add(newPiece);
                }
            }

            MergePiecesToTargetCount(targetCount);
        }
    
        private void MergePiecesToTargetCount(int targetCount)
        {
            while (_pieces.Count > targetCount)
            {
                int smallestIndex = 0;
                for (int i = 1; i < _pieces.Count; i++)
                {
                    if (_pieces[i].TriangleCount < _pieces[smallestIndex].TriangleCount)
                        smallestIndex = i;
                }
            
                int neighborPieceIndex = FindNeighborPiece(smallestIndex);
                if (neighborPieceIndex == -1)
                {
                    neighborPieceIndex = (smallestIndex + 1) % _pieces.Count;
                }
            
                var smallestPiece = _pieces[smallestIndex];
                _pieces[neighborPieceIndex].AddTriangles(smallestPiece.Triangles);
                _pieces.RemoveAt(smallestIndex);
            }
        }
    
        private int FindNeighborPiece(int smallestPieceIndex)
        {
            Data.Piece smallestPiece = _pieces[smallestPieceIndex];
        
            for (int i = 0; i < _pieces.Count; i++)
            {
                if (i == smallestPieceIndex) continue;
            
                if (smallestPiece.IsNeighborWith(_pieces[i]))
                    return i;
            }
        
            return -1; 
        }

        private void CollectSnapPoints()
        {
            HashSet<Vector3> uniquePoints = new HashSet<Vector3>();
            _snapPoints.Clear();
        
            foreach (var triangle in _allTriangles)
            {
                Vector3 headVertex = triangle.Vertices[0];
                uniquePoints.Add(headVertex);
            }
        
            _snapPoints.AddRange(uniquePoints);
        }
    
        private void FindNeighborTriangles()
        {
            _triangleNeighborService.FindNeighbors(_allTriangles);
        }
    
        private void GrowRegion(TriangleCell start, Data.Piece piece)
        {
            Queue<TriangleCell> queue = new Queue<TriangleCell>();
            queue.Enqueue(start);
            start.Visited = true;

            while (queue.Count > 0)
            {
                var currentTriangleCell = queue.Dequeue();
                piece.AddTriangle(currentTriangleCell);

                foreach (var triangleCell in currentTriangleCell.Neighbors)
                {
                    if (!triangleCell.Visited && Random.value > 0.6f)
                    {
                        triangleCell.Visited = true;
                        queue.Enqueue(triangleCell);
                    }
                }
            }
        }
    
    }
}