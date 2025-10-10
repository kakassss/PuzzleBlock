using Core.Popup.Scripts.Controller;
using UnityEngine;

namespace Core.Level.Controller
{
    public class LevelCompletionController : ILevelCompletionController
    {
        private readonly IPopupController _popupController;
        private readonly string _levelWinPopupName = "P_Screen_LevelWin"; 
    
        private int _totalPieces;
        private int _placedPieces;

        public LevelCompletionController(IPopupController popupController)
        {
            _popupController = popupController;
        }
    
        public void RegisterPiecePlacement(bool isPlaced)
        {
            if (isPlaced)
                _placedPieces++;
            else
                _placedPieces--;
        
            CheckCompletion();
        }

        public void SetLevelTarget(int totalPieces)
        {
            _totalPieces = totalPieces;
            _placedPieces = 0;
        
            Debug.Log($"Level progress reset: 0/{_totalPieces} pieces placed");
        }

        private bool IsLevelComplete()
        {
            return _totalPieces > 0 && _placedPieces == _totalPieces;
        }
    
        private float GetCompletionPercentage()
        {
            if (_totalPieces == 0) return 0f;
            return (float)_placedPieces / _totalPieces * 100f;
        }

        private void CheckCompletion()
        {
            if (IsLevelComplete())
            {
                _popupController.OpenPopupByName(_levelWinPopupName);
                Debug.Log("LEVEL COMPLETED!");
            }
            else
            {
                Debug.Log($"Progress: {_placedPieces}/{_totalPieces} pieces placed ({GetCompletionPercentage():F1}%)");
            }
        }
    }
}
