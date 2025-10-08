using UnityEngine;

public class ICameraSizeSizeController : ICameraSizeController
{
    private readonly ICameraService _cameraService;
    
    private float _cameraZPosition = -10f;
    private float _verticalOffset = 6.75f;
    private float _orthographicSize = 8f;

    public ICameraSizeSizeController(ICameraService cameraService)
    {
        _cameraService = cameraService;
    }
    
    public void SetCameraSize(int gridSize)
    {
        var camera = _cameraService.GetCamera();
        
        camera.orthographicSize = _orthographicSize;
        float posX = gridSize / 2f;
        float posY = gridSize - _verticalOffset;
        
        Vector3 targetPosition = new Vector3(posX, posY, _cameraZPosition);
        camera.transform.position = targetPosition;
    }
}
