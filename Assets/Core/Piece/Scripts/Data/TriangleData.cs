using System;
using UnityEngine;

[Serializable]
public class TriangleData
{
    public Vector3[] vertices;
    public Vector2 cell;
    
    public TriangleData(Vector3[] verts, Vector2 cellPos)
    {
        vertices = verts;
        cell = cellPos;
    }
}