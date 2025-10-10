using System.Collections.Generic;
using Core.Piece.Scripts.Data;

namespace Core.Grid.Scripts.Data
{
    public class GridCell
    {
        private List<TriangleCell> _triangles = new List<TriangleCell>();
        
        
        public bool CanPlace(TriangleCell triangle)
        {
            return _triangles.Count < 4 && !_triangles.Contains(triangle);
        }
    
        public void AddTriangle(TriangleCell triangle)
        {
            if (!_triangles.Contains(triangle))
            {
                _triangles.Add(triangle);
            }
        }
    
        public void RemoveTriangle(TriangleCell triangle)
        {
            _triangles.Remove(triangle);
        }
    
        public bool HasTriangle(TriangleCell triangle)
        {
            return _triangles.Contains(triangle);
        }
    }
}