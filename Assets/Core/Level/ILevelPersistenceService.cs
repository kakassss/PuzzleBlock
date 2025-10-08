public interface ILevelPersistenceService
{
    public void SaveLevel(LevelData levelData, string levelName);
    public LevelData LoadLevel(string levelName);
    public void SaveLastPlayedLevel(string levelName);
    public string GetLastPlayedLevel();
    public void DeleteLevel(string levelName);
}