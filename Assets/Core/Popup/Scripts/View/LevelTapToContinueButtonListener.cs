using Core.Button.Scripts;
using Core.GameInitializer;
using Core.GameInitializer.Scripts.Controller;
using Core.Popup.Scripts.Controller;
using Zenject;

namespace Core.Popup.Scripts.View
{
    public class LevelTapToContinueButtonListener : BaseButtonListener
    {
        private IPopupController _popupController;
        private IGameInitializer _gameInitializer;
    
        [Inject]
        private void Construct(IGameInitializer gameInitializer, IPopupController popupController)
        {
            _gameInitializer = gameInitializer;
            _popupController = popupController;
        }
    
        protected override void OnClick()
        {
            _popupController.CloseActivePopup();
        
            _gameInitializer.Initialize();   
        }
    }
}
