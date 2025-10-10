using UnityEngine;

namespace Core.Piece.Scripts.Controller.Interfaces
{
    public interface IPieceMouseInputHandler
    {
        public void HandleMouseDown(Transform transform);
        public void HandleMouseDrag(Transform transform);
        public void HandleMouseUp(Transform transform);
        public Vector3 GetMouseWorldPosition();
        public bool MouseWorldPosInGrid();
    }
}