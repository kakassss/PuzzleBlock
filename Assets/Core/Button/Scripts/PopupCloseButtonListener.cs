using Zenject;

public class PopupCloseButtonListener : BaseButtonListener
{
    private IPopupController _popupController;
    
    [Inject]
    private void Construct(IPopupController popupController)
    {
        _popupController = popupController;
    }
    
    protected override void OnClick()
    {
        _popupController.CloseActivePopup();   
    }
}
