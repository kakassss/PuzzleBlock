
public interface IGameDifficultyController
{
    public void SetDifficultyData(GameDifficultySOData gameDifficultySoData);
    public GameDifficultySOData GetDifficultyData();
}

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