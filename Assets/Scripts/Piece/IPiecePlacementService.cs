using System.Collections.Generic;
using UnityEngine;

public interface IPiecePlacementService
{
    public void Initialize(List<TriangleCell> allTriangles, Vector3 centerPosition);
    public bool TryPlaceOnGrid(Transform transform);
    public void ClearFromGrid();
}