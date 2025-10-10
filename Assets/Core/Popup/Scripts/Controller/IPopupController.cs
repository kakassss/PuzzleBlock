namespace Core.Popup.Scripts.Controller
{
    public interface IPopupController
    {
        public void OpenPopupByName(string name);
        public void CloseActivePopup();
    }
}