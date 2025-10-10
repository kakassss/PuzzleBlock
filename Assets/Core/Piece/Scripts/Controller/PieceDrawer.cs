using System.Collections.Generic;
using Core.Level.Controller;
using Core.Piece.Scripts.Controller.Interfaces;
using Core.Piece.Scripts.View;
using UnityEngine;
using Zenject;

namespace Core.Piece.Scripts.Controller
{
    public class PieceDrawer : IPieceDrawer
    {
        private IInstantiator _instantiator;
        private IPieceSpawnPositionController _pieceSpawnPositionController;
        private ILevelCompletionController _levelCompletionController;

        public PieceDrawer(IPieceSpawnPositionController pieceSpawnPositionController,
            ILevelCompletionController levelCompletionController, IInstantiator instantiator)
        {
            _instantiator = instantiator;
            _levelCompletionController = levelCompletionController;
            _pieceSpawnPositionController = pieceSpawnPositionController;
        }

        public List<PieceView> DrawPieces(List<Data.Piece> pieces, List<Vector3> snapPoints,PieceView pieceViewPrefab, Transform parent)
        {
            List<PieceView> spawnedPieces = new List<PieceView>();

            for (int i = 0; i < pieces.Count; i++)
            {
                Data.Piece piece = pieces[i];
                Mesh mesh = piece.CreateMesh();

                var pieceGo = _instantiator.InstantiatePrefabForComponent<PieceView>(pieceViewPrefab, parent);
                pieceGo.name = "Piece_" + piece.ID;
                mesh.name = "Piece_" + piece.ID + "_Mesh";

                pieceGo.SetPiece(piece.Triangles, snapPoints);
                pieceGo.SetMesh(mesh);

                pieceGo.transform.position = _pieceSpawnPositionController.GetSpawnPosition();
                _pieceSpawnPositionController.PieceMovementTween(pieceGo.transform);
            
                spawnedPieces.Add(pieceGo);
            }

            _levelCompletionController.SetLevelTarget(spawnedPieces.Count);
        
            return spawnedPieces;
        }
    }
}
