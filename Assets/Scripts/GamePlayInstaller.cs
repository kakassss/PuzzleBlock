using UnityEngine;
using Zenject;

public class GamePlayInstaller : MonoInstaller
{
    [Header("References")]
    [SerializeField] private Camera _mainCamera;
    
    [Header("Grid Settings")]
    [SerializeField] [Range(4,6)] private int GridSize = 5;
    [SerializeField] private float gridCellSize;
    public override void InstallBindings()
    {
        BindControllers();
    }
    
    private void BindControllers()
    {
        Container.BindInterfacesTo<Grid>().AsSingle().WithArguments(GridSize,gridCellSize).NonLazy();
        Container.BindInterfacesTo<CameraService>().AsSingle().WithArguments(_mainCamera).NonLazy();
        Container.BindInterfacesTo<PieceZOrderService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<PieceMouseInputHandler>().AsSingle().NonLazy();
        Container.BindInterfacesTo<PieceSpawnPositionService>().AsSingle().NonLazy();
        
        Container.BindInterfacesTo<LevelPersistenceService>().AsSingle().NonLazy();
    }
}
