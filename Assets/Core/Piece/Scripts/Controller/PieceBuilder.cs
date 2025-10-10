using System.Collections.Generic;
using Core.Level.Controller;
using Core.Piece.Scripts.Controller.Interfaces;
using Core.Piece.Scripts.View;
using UnityEngine;
using Zenject;

namespace Core.Piece.Scripts.Controller
{
    public class PieceBuilder : IPieceBuilder
    {
        private PieceView _pieceViewPrefab;
        private Transform _pieceViewParent;
    
        private IInstantiator _instantiator;
        private IPieceSpawnPositionController _pieceSpawnPositionController;
        private IPieceFactory _pieceFactory;
        private IPieceSaverService _pieceSaverService;
        private ILevelCompletionController _levelCompletionController;
    
        public PieceBuilder(IInstantiator instantiator, IPieceSpawnPositionController pieceSpawnPositionService, IPieceFactory pieceFactory,
            PieceView pieceViewPrefab, Transform pieceViewParent, IPieceSaverService pieceSaverService, ILevelCompletionController levelCompletionController)
        {
            _instantiator = instantiator;    
            _pieceSpawnPositionController = pieceSpawnPositionService;
            _pieceFactory = pieceFactory;
            _pieceSaverService = pieceSaverService;
            _levelCompletionController = levelCompletionController;

            _pieceViewPrefab = pieceViewPrefab;
            _pieceViewParent = pieceViewParent;
        }
    
        public void GenerateNewPiece()
        {
            _pieceFactory.GeneratePiece();
            DrawPieces();
        }
    
        private void DrawPieces()
        {
            List<PieceView> spawnedPieces = new List<PieceView>();;
            List<global::Core.Piece.Scripts.Data.Piece> pieces = _pieceFactory.GetPieces();
            List<Vector3> snapPoints = _pieceFactory.GetSnapPoints();
        
            for (int i = 0; i < pieces.Count; i++)
            {
                global::Core.Piece.Scripts.Data.Piece piece = pieces[i];
                Mesh mesh = piece.CreateMesh();
            
                var pieceGo = _instantiator.InstantiatePrefabForComponent<PieceView>(_pieceViewPrefab, _pieceViewParent);
                pieceGo.name = "Piece_" + piece.ID;
                mesh.name = "Piece_" + piece.ID + "_Mesh";
            
                pieceGo.SetPiece(piece.Triangles,snapPoints);
                pieceGo.SetMesh(mesh);
            
                pieceGo.transform.position = _pieceSpawnPositionController.GetSpawnPosition();
                _pieceSpawnPositionController.PieceMovementTween(pieceGo.transform);
                spawnedPieces.Add(pieceGo);
            }
        
            _pieceSaverService.SavePieceData(spawnedPieces, pieces,snapPoints);
            _levelCompletionController.SetLevelTarget(spawnedPieces.Count);
        }

        public void Clear()
        {
            _pieceSaverService.Clear();
        }
    }
}