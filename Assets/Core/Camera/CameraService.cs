using UnityEngine;

public class CameraService : ICameraService
{
    private readonly Camera _camera;

    public CameraService(Camera camera)
    {
        _camera = camera;
    }
    
    public Camera GetCamera()
    {
        return _camera;
    }
}