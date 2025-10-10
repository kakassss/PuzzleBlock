using System.Collections.Generic;
using Core.Popup.Scripts.Controller;
using Core.Popup.Scripts.Data;
using UnityEngine;
using Zenject;

public class PopupController : IPopupController
{
    private readonly IInstantiator _instantiator;
    private readonly Transform _popupContainerParent;
    
    private Dictionary<string, GameObject> _allPopups = new Dictionary<string, GameObject>();
    private List<PopupData> _popups = new List<PopupData>();

    private GameObject _activePopup;
    
    public PopupController(IInstantiator instantiator, Transform popupContainerParent, List<PopupData> popups)
    {
        _instantiator = instantiator;
        _popupContainerParent = popupContainerParent;
        _popups = popups;
        
        foreach (var popup in _popups)
        {
            _allPopups.Add(popup.Name, popup.Prefab);
        }
    }
    
    public void OpenPopupByName(string name)
    {
        if(_activePopup != null) return;
        
        _activePopup = _instantiator.InstantiatePrefab(GetPopupByName(name), _popupContainerParent);
    }
    
    public void CloseActivePopup()
    {
        if (_activePopup == null) return;
        
        _activePopup.gameObject.SetActive(false);
        Object.Destroy(_activePopup.gameObject);
        _activePopup = null;
    }
    
    private GameObject GetPopupByName(string name)
    {
        return _allPopups[name];
    }
}