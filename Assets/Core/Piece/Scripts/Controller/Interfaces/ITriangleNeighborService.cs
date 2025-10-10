using System.Collections.Generic;
using Core.Piece.Scripts.Data;

namespace Core.Piece.Scripts.Controller.Interfaces
{
    public interface ITriangleNeighborService
    {
        void FindNeighbors(List<TriangleCell> triangles);
    }
}