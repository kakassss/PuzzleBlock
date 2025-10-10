using Core.Piece.Scripts.Controller.Interfaces;
using UnityEngine;

namespace Core.Piece.Scripts.Controller
{
    public class PieceZOrderController : IPieceZOrderController
    {
        private static float GlobalZOrder = 0;
    
        public void BringToFront(Transform transform)
        {
            GlobalZOrder -= 0.01f;
            Vector3 pos = transform.position;
            pos.z = GlobalZOrder;
            transform.position = pos;
        }
    }
}