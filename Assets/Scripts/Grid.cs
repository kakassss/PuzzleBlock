using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Grid : IGridController
{
    private const int GRID_SIZE_OFFSET = 1;
    
    private int _gridSize;
    private float _cellSize;
    private GridCell[,] _gridArray;
    private List<Vector3> _cellVisualPosition;
    private List<Vector3> _centerPos;
    
    public Grid(int gridSize, float cellSize)
    {
        _gridSize = gridSize;
        _cellSize = cellSize;
        _cellVisualPosition = new List<Vector3>();
        _centerPos = new List<Vector3>();
        _gridArray = new GridCell[_gridSize, _gridSize];

        for (int i = 0; i < _gridSize + GRID_SIZE_OFFSET; i++)
        {
            for (int j = 0; j < _gridSize + GRID_SIZE_OFFSET; j++)
            {
                _cellVisualPosition.Add(GetWorldPosition(i,j));
            }
        }
        
        for (int x = 0; x < _gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < _gridArray.GetLength(1); y++)
            {
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x, y + 1), Color.green,100f);
                Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x+1, y), Color.green,100f);
                
                Vector3 topLeft = new Vector3(x, y + 1, 0);             
                Vector3 topRight = new Vector3(x + 1, y + 1, 0);     
                Vector3 bottomLeft = new Vector3(x, y, 0);                
                Vector3 bottomRight = new Vector3(x + 1, y, 0);             
                Vector3 center = (bottomLeft + bottomRight + topLeft + topRight) / 4f;
                
                _gridArray[x, y] = new GridCell(new Vector3(x,y));
                
                _centerPos.Add(center);
            }
        }
        
        Debug.DrawLine(GetWorldPosition(0,_gridSize), GetWorldPosition(_gridSize, _gridSize), Color.green,100f);
        Debug.DrawLine(GetWorldPosition(_gridSize,0), GetWorldPosition(_gridSize, _gridSize), Color.green,100f);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x,y) * _cellSize;
    }

    public void SetValue(int x, int y, GridCell value)
    {
        if(x >= 0 && y >= 0  && x < _gridSize  && y < _gridSize)
            _gridArray[x, y] = value;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt(worldPosition.x / _cellSize);
        y = Mathf.FloorToInt(worldPosition.y / _cellSize);
    }
    
    public void SetValue(Vector3 worldPositon, GridCell value)
    {
        int x, y;
        GetXY(worldPositon, out x, out y);
        SetValue(x, y, value);
    }

    public int GetGridSize()
    {
        return _gridSize;
    }

    public GridCell GetValue(int x, int y)
    {
        if(x >= 0 && y >= 0 && x < _gridSize && y < _gridSize)
            return _gridArray[x, y];
        
        return null;
    }

    public GridCell GetValue(Vector3 worldPositon)
    {
        int x, y;
        GetXY(worldPositon, out x, out y);
        return GetValue(x, y);
    }

    public List<Vector3> GetGridVisualPositions()
    {
        return _cellVisualPosition;
    }

    public List<Vector3> GetCenterPositions()
    {
        return _centerPos;
    }
}