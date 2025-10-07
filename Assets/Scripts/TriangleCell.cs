using System.Collections.Generic;
using UnityEngine;

public class TriangleCell
{
    public Vector3 Cell;
    public Vector3[] Vertices;
    public List<TriangleCell> Neighbors = new List<TriangleCell>();
    public bool Visited;
    
    public TriangleCell(Vector3[] vertices, bool visited, Vector2 cell)
    {
        Vertices = vertices;
        Visited = visited;
        Cell = cell;
    }
}