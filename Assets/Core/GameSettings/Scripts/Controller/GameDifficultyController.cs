using Core.GameSettings.Scripts.Data;

namespace Core.GameSettings.Scripts.Controller
{
    public class GameDifficultyController : IGameDifficultyController
    {
        private GameDifficultySOData _currentGameDifficultySoData;
    
        public void SetDifficultyData(GameDifficultySOData gameDifficultySoData)
        {
            _currentGameDifficultySoData = gameDifficultySoData;
        }
    
        public GameDifficultySOData GetDifficultyData()
        {
            return _currentGameDifficultySoData;
        }
    }
}