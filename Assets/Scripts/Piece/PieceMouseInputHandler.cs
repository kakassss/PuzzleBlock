using UnityEngine;

public class PieceMouseInputHandler : IPieceMouseInputHandler
{
    private Vector3 _dragOffset;
    private Vector3 _originalPosition;

    private ICameraService _cameraService;
    private IGridController _gridController;

    public PieceMouseInputHandler(IGridController gridController, ICameraService cameraService)
    {
        _gridController = gridController;
        _cameraService = cameraService;
    }
    
    public void HandleMouseDown(Transform transform)
    {
        _originalPosition = transform.position;
        
        Vector3 mousePos = GetMouseWorldPosition();
        _dragOffset = transform.position - mousePos;
    }
    
    public void HandleMouseDrag(Transform transform)
    {
        Vector3 mousePos = GetMouseWorldPosition();
        transform.position = mousePos + _dragOffset;
    }

    public void HandleMouseUp(Transform transform)
    {
        transform.position = _originalPosition;
    }
    
    public bool MouseWorldPosInGrid()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();
        int x, y;
        _gridController.GetXY(mouseWorldPos, out x, out y);
        
        return _gridController.InBounds(x, y);
    }
    
    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Mathf.Abs(_cameraService.GetCamera().transform.position.z);
        return _cameraService.GetCamera().ScreenToWorldPoint(mousePos);
    }
}