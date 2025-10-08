
public class GameInitializer : IGameInitializer
{
    private readonly IGridController _gridController;
    private readonly ICameraController _cameraController;
    
    public GameInitializer(
        IGridController gridController,
        ICameraController cameraController)
    {
        _gridController = gridController;
        _cameraController = cameraController;
        
        Initialize();
    }
    
    public void Initialize()
    {
        SetupGame();
    }
    
    private void SetupGame()
    {
        int gridSize = _gridController.GetGridSize();
        _cameraController.SetCameraSize(gridSize);
    }
}
