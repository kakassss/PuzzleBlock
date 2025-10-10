using UnityEngine;

namespace Core.Popup.Scripts.Data
{
    [CreateAssetMenu(fileName = "PopupData", menuName = "ScriptableObjects/PopupDataSO")]
    public class PopupData : ScriptableObject
    {
        public string Name;
        public GameObject Prefab;
    }
}