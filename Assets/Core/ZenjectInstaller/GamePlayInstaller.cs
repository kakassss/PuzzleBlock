using System.Collections.Generic;
using Core.Camera;
using Core.Grid.Scripts.Controller;
using Core.Level.Controller;
using Core.Piece.Scripts.Controller;
using Core.Piece.Scripts.View;
using Core.Popup.Scripts.Controller;
using Core.Popup.Scripts.Data;
using UnityEngine;
using Zenject;

namespace Core.ZenjectInstaller
{
    public class GamePlayInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private UnityEngine.Camera _mainCamera;
    
        [Header("Popup Settings")] 
        [SerializeField] private List<PopupData> _popups;
        [SerializeField] private Transform _popupContainerParent;
    
        [Header("Piece Settings")]
        [SerializeField] private PieceView _pieceViewPrefab;
        [SerializeField] private Transform _pieceParent;
    
        public override void InstallBindings()
        {
            BindControllers();
        }
    
        private void BindControllers()
        {
            //Base Feature Containers
            Container.BindInterfacesTo<GridController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<LevelController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PopupController>().AsSingle().WithArguments(_popups,_popupContainerParent).NonLazy();
        
            //Service Containers
            Container.BindInterfacesTo<LevelPersistenceService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CameraService>().AsSingle().WithArguments(_mainCamera).NonLazy();
            Container.BindInterfacesTo<CameraSizeController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<LevelCompletionController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<GameInitializer.GameInitializer>().AsSingle().NonLazy();
            //Piece Containers
            Container.BindInterfacesTo<PieceMouseInputHandler>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PieceSpawnPositionController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PieceZOrderController>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PieceSaverService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<TriangleNeighborService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<PieceLoader>().AsSingle().WithArguments(_pieceViewPrefab,_pieceParent).NonLazy();
            Container.BindInterfacesTo<PieceFactory>().AsTransient().NonLazy();
            Container.BindInterfacesTo<PieceBuilder>().AsSingle().WithArguments(_pieceViewPrefab,_pieceParent).NonLazy();
            
            Container.BindInterfacesTo<PieceDrawer>().AsSingle().NonLazy();
        }
    }
}
