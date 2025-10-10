using Core.GameSettings.Scripts.Controller;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<GameDifficultyController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<SceneLoaderService>().AsSingle().NonLazy();
    }
    
}
