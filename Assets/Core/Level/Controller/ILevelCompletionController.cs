namespace Core.Level.Controller
{
    public interface ILevelCompletionController
    {
        public void RegisterPiecePlacement(bool isPlaced);
        public void SetLevelTarget(int totalPieces);
    }
}