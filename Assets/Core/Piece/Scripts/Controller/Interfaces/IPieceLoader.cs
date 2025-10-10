using Core.Level.Data;

namespace Core.Piece.Scripts.Controller.Interfaces
{
    public interface IPieceLoader
    {
        public void LoadFromLevelData(LevelData levelData);
        public void Clear();
    }
}
