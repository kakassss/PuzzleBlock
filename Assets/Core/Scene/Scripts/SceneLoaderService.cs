using UnityEngine.SceneManagement;

public interface ISceneLoaderService
{
    public void LoadGameScene();
    public void LoadMenuScene();
}

public class SceneLoaderService : ISceneLoaderService
{
    private const string MenuSceneName = "MenuScene";
    private const string GameSceneName = "GameScene";
    
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene(GameSceneName);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene(MenuSceneName);
    }
}
