using Core.Piece.Scripts.Controller.Interfaces;
using DG.Tweening;
using UnityEngine;

namespace Core.Piece.Scripts.Controller
{
    public class PieceSpawnPositionController : IPieceSpawnPositionController
    {
        public Vector3 GetSpawnPosition()
        {
            float randomX = Random.Range(2, 3);
            float randomY = Random.Range(10, 13);
        
            return new Vector3(randomX, randomY, 0);
        }

        public void PieceMovementTween(Transform transform)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.SetAutoKill(true);
        
            float randomX = Random.Range(-0.5f,1f);
            float randomY = Random.Range(-14,-16);
            var targetPos = transform.position + new Vector3(randomX,randomY);
        
            sequence.Append(transform.DOMove(targetPos, .5f));
        }
    }
}