using UnityEngine;
using Zenject;

public class GamePlayInstaller : MonoInstaller
{
    [Header("References")]
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PieceView _pieceViewPrefab;
    [SerializeField] private Transform _pieceParent;
    
    [Header("Piece ")]
    [SerializeField] private int _pieceCount;
    
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
        Container.BindInterfacesTo<PieceZOrderController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<PieceMouseInputHandler>().AsSingle().NonLazy();
        Container.BindInterfacesTo<PieceSpawnPositionController>().AsSingle().NonLazy();
        
        Container.BindInterfacesTo<LevelPersistenceService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<ICameraSizeSizeController>().AsSingle().NonLazy();
        Container.BindInterfacesTo<TriangleNeighborService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<PieceSaverService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<PieceLoader>().AsSingle().WithArguments(_pieceViewPrefab,_pieceParent).NonLazy();
        Container.BindInterfacesTo<PieceBuilder>().AsSingle().WithArguments(_pieceViewPrefab,_pieceParent).NonLazy();
        Container.BindInterfacesTo<GameInitializer>().AsSingle().NonLazy();
        Container.BindInterfacesTo<PieceFactory>().AsTransient().WithArguments(_pieceCount).NonLazy();
    }
}
