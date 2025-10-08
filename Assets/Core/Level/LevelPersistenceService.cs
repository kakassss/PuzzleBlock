using System.IO;
using UnityEngine;

public class LevelPersistenceService : ILevelPersistenceService
{
    private string SavePath => Path.Combine(Application.persistentDataPath, "Levels");
    
    public void SaveLevel(LevelData levelData, string levelName)
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        
        string json = JsonUtility.ToJson(levelData, true);
        string filePath = Path.Combine(SavePath, $"{levelName}.json");
        
        File.WriteAllText(filePath, json);
        Debug.Log($"Level saved to: {filePath}");
    }
    
    public LevelData LoadLevel(string levelName)
    {
        string filePath = Path.Combine(SavePath, $"{levelName}.json");
        
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Level file not found: {filePath}");
            return null;
        }
        
        string json = File.ReadAllText(filePath);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);
        
        Debug.Log($"Level loaded from: {filePath}");
        return levelData;
    }
    
    public void SaveLastPlayedLevel(string levelName)
    {
        string filePath = Path.Combine(SavePath, "last_played.txt");
        
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        
        File.WriteAllText(filePath, levelName);
    }
    
    public string GetLastPlayedLevel()
    {
        string filePath = Path.Combine(SavePath, "last_played.txt");
        
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath).Trim();
        }
        
        return "";
    }
    
    public void DeleteLevel(string levelName)
    {
        string filePath = Path.Combine(SavePath, $"{levelName}.json");
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"Level deleted: {levelName}");
        }
        else
        {
            Debug.LogWarning($"Level not found for deletion: {levelName}");
        }
    }
}