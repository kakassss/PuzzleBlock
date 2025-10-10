using System.Collections.Generic;
using Core.Level.Data;
using Core.Piece.Scripts.Controller.Interfaces;
using Core.Piece.Scripts.Data;
using Core.Piece.Scripts.View;
using UnityEngine;

namespace Core.Piece.Scripts.Controller
{
    public class PieceLoader : IPieceLoader
    {
        private List<TriangleCell> _allTriangles = new List<TriangleCell>();
        private List<Data.Piece> _pieces = new List<Data.Piece>();
        private List<Vector3> _snapPoints = new List<Vector3>();
        private List<PieceView> _spawnedPieces = new List<PieceView>();

        private Transform _parentTransform;
        private PieceView _pieceViewPrefab;
    
        private IPieceTriangleNeighborService _ıPieceTriangleNeighborService;
        private IPieceDrawer _pieceDrawer;

        public PieceLoader(PieceView pieceViewPrefab,Transform parentTransform,IPieceTriangleNeighborService ıPieceTriangleNeighborService,
             IPieceDrawer pieceDrawer)
        {
            _ıPieceTriangleNeighborService = ıPieceTriangleNeighborService;
            _pieceDrawer = pieceDrawer;
        
            _parentTransform = parentTransform;
            _pieceViewPrefab = pieceViewPrefab;
        
        }
    
        public void LoadFromLevelData(LevelData levelData)
        {
            _snapPoints = new List<Vector3>(levelData.SnapPoints);
        
            foreach (var pieceData in levelData.Pieces)
            {
                Data.Piece piece = new Data.Piece(pieceData.pieceId);
            
                foreach (var triData in pieceData.triangles)
                {
                    TriangleCell triangle = new TriangleCell(triData.vertices, false, triData.cell);
                    piece.AddTriangle(triangle);
                    _allTriangles.Add(triangle);
                }
            
                _pieces.Add(piece);
            }
        
            _ıPieceTriangleNeighborService.FindNeighbors(_allTriangles);
            DrawLoadedPieces();
        }
        
        private void DrawLoadedPieces()
        {
            _spawnedPieces = _pieceDrawer.DrawPieces(_pieces, _snapPoints,_pieceViewPrefab, _parentTransform);
        }
        
        public void Clear()
        {
            if (_spawnedPieces is { Count: > 0 })
            {
                foreach (var piece in _spawnedPieces)
                {
                    Object.Destroy(piece.gameObject);
                }    
            }
        
            _allTriangles.Clear();
            _pieces.Clear();
            _spawnedPieces.Clear();
        }
    }
}