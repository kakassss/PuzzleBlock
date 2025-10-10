using UnityEngine;

namespace Core.Button.Scripts
{
    public abstract class BaseButtonListener : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Button _button;

        private void Awake()
        {
            AddListener();
        }

        private void AddListener()
        {
            _button.onClick.AddListener(OnClick);
        }
    
        protected abstract void OnClick();
    }
}
