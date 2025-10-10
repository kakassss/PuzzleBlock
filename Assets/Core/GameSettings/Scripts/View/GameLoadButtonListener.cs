using Core.Button.Scripts;
using Core.Level.Controller;
using Zenject;

namespace Core.GameSettings.Scripts.View
{
    public class GameLoadButtonListener : BaseButtonListener
    {
        private ILevelController _levelController;
    
        [Inject]
        private void Construct(ILevelController levelController)
        {
            _levelController = levelController;
        }
    
        protected override void OnClick()
        {
            _levelController.LoadLevel();
        }
    }
}
