using Core.Camera;
using Core.Camera.Scripts.Controller;
using Core.Grid.Scripts.Controller;
using Core.Level.Controller;

namespace Core.GameInitializer.Scripts.Controller
{
    public class GameInitializer : IGameInitializer
    {
        private readonly IGridController _gridController;
        private readonly ICameraSizeController _cameraSizeController;
        private readonly ILevelController _levelController;
    
        public GameInitializer(IGridController gridController, ICameraSizeController cameraSizeController, ILevelController levelController)
        {
            _gridController = gridController;
            _cameraSizeController = cameraSizeController;
            _levelController = levelController;
        
            Initialize();
        }
    
        public void Initialize()
        {
            SetupGame();
        }
    
        private void SetupGame()
        {
            int gridSize = _gridController.GetGridSize();
            _cameraSizeController.SetCameraSize(gridSize);
        
            _levelController.GenerateNewLevel();
        }
    }
}
