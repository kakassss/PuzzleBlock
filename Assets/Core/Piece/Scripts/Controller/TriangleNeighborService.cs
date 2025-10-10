using System.Collections.Generic;
using Core.Piece.Scripts.Controller.Interfaces;
using Core.Piece.Scripts.Data;
using UnityEngine;

namespace Core.Piece.Scripts.Controller
{
    public class TriangleNeighborService : ITriangleNeighborService
    {
        private float _epsilon = 0.01f;
    
        public void FindNeighbors(List<TriangleCell> triangles)
        {
            foreach (var triangleX in triangles)
            {
                triangleX.Neighbors.Clear();
            
                foreach (var triangleY in triangles)
                {
                    if (triangleX == triangleY) continue;
                
                    int sharedVertices = 0;
                
                    foreach (var va in triangleX.Vertices)
                    {
                        foreach (var vb in triangleY.Vertices)
                        {
                            if (Vector3.Distance(va, vb) < _epsilon)
                                sharedVertices++;
                        }
                    }

                    if (sharedVertices == 2)
                    {
                        if (!triangleX.Neighbors.Contains(triangleY))
                            triangleX.Neighbors.Add(triangleY);
                    }
                }
            }
        }
    }
}