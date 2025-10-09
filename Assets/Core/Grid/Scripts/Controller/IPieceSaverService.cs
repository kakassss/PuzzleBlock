using System.Collections.Generic;
using UnityEngine;

public interface IPieceSaverService
{
    public void SavePieceData(List<PieceView> spawnedPieces, List<Piece> pieces, List<Vector3> snapPoints);
    public List<PieceView> GetSpawnedPieces();
    public List<Piece> GetPieces();
    public List<Vector3> GetSnapPoints();
    public void Clear();
}