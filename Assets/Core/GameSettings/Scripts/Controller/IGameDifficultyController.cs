using Core.GameSettings.Scripts.Data;

namespace Core.GameSettings.Scripts.Controller
{
    public interface IGameDifficultyController
    {
        public void SetDifficultyData(GameDifficultySOData gameDifficultySoData);
        public GameDifficultySOData GetDifficultyData();
    }
}