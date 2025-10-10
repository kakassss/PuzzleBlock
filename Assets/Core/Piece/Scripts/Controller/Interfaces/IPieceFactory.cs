using System.Collections.Generic;
using UnityEngine;

public interface IPieceFactory
{
    public void GeneratePiece();
    public int GetGridSize();
    public List<Vector3> GetSnapPoints();
    public List<Piece> GetPieces();
}