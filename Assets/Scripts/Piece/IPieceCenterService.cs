using System.Collections.Generic;
using UnityEngine;

public interface IPieceCenterService
{
    public void CalculatePieceCenter(List<TriangleCell> allTriangles);
    public Vector3 GetPieceCenter();
}