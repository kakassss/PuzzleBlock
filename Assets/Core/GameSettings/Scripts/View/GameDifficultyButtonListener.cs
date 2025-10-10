using Core.Button.Scripts;
using Core.GameSettings.Scripts.Controller;
using Core.GameSettings.Scripts.Data;
using UnityEngine;
using Zenject;

namespace Core.GameSettings.Scripts.View
{
    public class GameDifficultyButtonListener : BaseButtonListener
    {
        [SerializeField] private GameDifficultySOData _gameDifficultySOData;
    
        private ISceneLoaderService _sceneLoaderService;
        private IGameDifficultyController _gameDifficultyController;

        [Inject]
        private void Construct(IGameDifficultyController gameDifficultyController, ISceneLoaderService sceneLoaderService)
        {
            _gameDifficultyController = gameDifficultyController;
            _sceneLoaderService = sceneLoaderService;
        }
    
        protected override void OnClick()
        {
            _gameDifficultyController.SetDifficultyData(_gameDifficultySOData);
        
            _sceneLoaderService.LoadGameScene();
        }
    }
}
