
public class GameInitializer : IGameInitializer
{
    private readonly IGridController _gridController;
    private readonly ICameraSizeController _ıCameraSizeController;
    
    public GameInitializer(
        IGridController gridController,
        ICameraSizeController ıCameraSizeController)
    {
        _gridController = gridController;
        _ıCameraSizeController = ıCameraSizeController;
        
        Initialize();
    }
    
    public void Initialize()
    {
        SetupGame();
    }
    
    private void SetupGame()
    {
        int gridSize = _gridController.GetGridSize();
        _ıCameraSizeController.SetCameraSize(gridSize);
    }
}
