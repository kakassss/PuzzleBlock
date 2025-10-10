namespace Core.Camera
{
    public class CameraService : ICameraService
    {
        private readonly UnityEngine.Camera _camera;

        public CameraService(UnityEngine.Camera camera)
        {
            _camera = camera;
        }
    
        public UnityEngine.Camera GetCamera()
        {
            return _camera;
        }
    }
}