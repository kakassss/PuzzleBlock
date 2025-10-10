using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Piece.Scripts.Data
{
    [Serializable]
    public class Piece
    {
        public int ID { get; private set; }
        private List<TriangleCell> _triangles;
    
        private Bounds? _bounds;
        private Dictionary<Vector3, int> _cellOccupancy;
    
        public Piece(int id)
        {
            ID = id;
            _triangles = new List<TriangleCell>();
            _cellOccupancy = new Dictionary<Vector3, int>();
        }
    
        public void AddTriangle(TriangleCell triangle)
        {
            if (!_triangles.Contains(triangle))
            {
                _triangles.Add(triangle);
            
                if (!_cellOccupancy.ContainsKey(triangle.Cell))
                    _cellOccupancy[triangle.Cell] = 0;
            
                _cellOccupancy[triangle.Cell]++;
            
                _bounds = null;
            }
        }
    
        public void AddTriangles(List<TriangleCell> triangles)
        {
            foreach (var tri in triangles)
            {
                AddTriangle(tri);
            }
        }
    
        public List<TriangleCell> Triangles => _triangles;
        public int TriangleCount => _triangles.Count;
        
        public Bounds GetBounds()
        {
            if (_bounds.HasValue)
                return _bounds.Value;
        
            if (_triangles.Count == 0)
                return new Bounds(Vector3.zero, Vector3.zero);
        
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        
            foreach (var tri in _triangles)
            {
                foreach (var vertex in tri.Vertices)
                {
                    min = Vector3.Min(min, vertex);
                    max = Vector3.Max(max, vertex);
                }
            }
        
            Vector3 center = (min + max) / 2f;
            Vector3 size = max - min;
        
            _bounds = new Bounds(center, size);
            return _bounds.Value;
        }
    
        public Vector3 GetCenter()
        {
            return GetBounds().center;
        }
    
        public bool IsNeighborWith(Piece other)
        {
            foreach (var tri in _triangles)
            {
                foreach (var neighbor in tri.Neighbors)
                {
                    if (other._triangles.Contains(neighbor))
                        return true;
                }
            }
            return false;
        }
        
        public Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
        
            Vector3 center = GetCenter();
    
            foreach (var tri in _triangles)
            {
                int startIndex = vertices.Count;
        
                vertices.Add(tri.Vertices[0] - center);
                vertices.Add(tri.Vertices[1] - center);
                vertices.Add(tri.Vertices[2] - center);
        
                triangles.Add(startIndex);
                triangles.Add(startIndex + 1);
                triangles.Add(startIndex + 2);
            }
    
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        
            return mesh;
        }
    }
}