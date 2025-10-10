using Zenject;

public class LevelTapToContinueButtonListener : BaseButtonListener
{
    private IPopupController _popupController;
    private IGameInitializer _gameInitializer;
    
    [Inject]
    private void Construct(IGameInitializer gameInitializer, IPopupController popupController)
    {
        _gameInitializer = gameInitializer;
        _popupController = popupController;
    }
    
    protected override void OnClick()
    {
        _popupController.CloseActivePopup();
        
        _gameInitializer.Initialize();   
    }
}
