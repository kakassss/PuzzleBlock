using UnityEngine;
using UnityEngine.UI;

public abstract class BaseButtonListener : MonoBehaviour
{
    [SerializeField] private Button _button;

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
