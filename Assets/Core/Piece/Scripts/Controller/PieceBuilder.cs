using System.Collections.Generic;
using Core.Piece.Scripts.Controller.Interfaces;
using Core.Piece.Scripts.View;
using UnityEngine;

namespace Core.Piece.Scripts.Controller
{
    public class PieceBuilder : IPieceBuilder
    {
        private PieceView _pieceViewPrefab;
        private Transform _pieceViewParent;
    
        private IPieceFactory _pieceFactory;
        private IPieceSaverService _pieceSaverService;
        private IPieceDrawer _pieceDrawer;

        public PieceBuilder( IPieceFactory pieceFactory, PieceView pieceViewPrefab, Transform pieceViewParent, 
            IPieceSaverService pieceSaverService,
             IPieceDrawer pieceDrawer)
        {
            _pieceFactory = pieceFactory;
            _pieceSaverService = pieceSaverService;
            _pieceDrawer = pieceDrawer;

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
            List<Data.Piece> pieces = _pieceFactory.GetPieces();
            List<Vector3> snapPoints = _pieceFactory.GetSnapPoints();

            List<PieceView> spawnedPieces = _pieceDrawer.DrawPieces(pieces, snapPoints,_pieceViewPrefab, _pieceViewParent);
    
            _pieceSaverService.SavePieceData(spawnedPieces, pieces, snapPoints);
        }
        
        public void Clear()
        {
            _pieceSaverService.Clear();
        }
    }
}