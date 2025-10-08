using System.Collections.Generic;
using UnityEngine;

public interface IPieceCenterController
{
    public void CalculatePieceCenter(List<TriangleCell> allTriangles);
    public Vector3 GetPieceCenter();
}