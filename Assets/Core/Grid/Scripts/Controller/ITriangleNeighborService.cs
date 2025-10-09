using System.Collections.Generic;

public interface ITriangleNeighborService
{
    void FindNeighbors(List<TriangleCell> triangles);
}