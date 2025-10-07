using System;
using UnityEngine;

public class MouseTest : MonoBehaviour
{
    private Grid grid;
    private Camera _camera;
    
    private GridCell gridCell;
    private void Start()
    {
        _camera = Camera.main;
    }

    private int x, y;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}

public static class MouseClick
{
    public static Vector3 GetMouseWorldPosition(Camera camera)
    {
        Vector3 pos = GetMouseWorldPositionWithZ(Input.mousePosition, camera);
        pos.z = 0;
        return pos;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition,Camera worldCamera)
    {
        return worldCamera.ScreenToWorldPoint(screenPosition);
    }
}