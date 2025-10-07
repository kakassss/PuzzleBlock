using UnityEngine;
using Zenject;

public class GamePlayInstaller : MonoInstaller
{
    [SerializeField] [Range(4,6)] private int GridSize = 5;

    [SerializeField] private float gridCellSize;
    public override void InstallBindings()
    {
        BindControllers();
    }
    
    private void BindControllers()
    {
        Container.BindInterfacesTo<Grid>().AsSingle().WithArguments(GridSize,gridCellSize).NonLazy();
    }
}
