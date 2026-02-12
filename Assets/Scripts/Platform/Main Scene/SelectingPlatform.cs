using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

namespace Platform.Main_Scene
{
    public class SelectingPlatform : MonoBehaviour
    {
        [SerializeField] private MainPlatform _mainPlatform;
        
        public int platformID;

        void Start()
        {
            Debug.Log($"Save Active Level : {GameSave.GetSettings().Level}, Platfor ID : {platformID}");
            if (GameSave.GetSettings().Level >= platformID)
            {
                _mainPlatform.Activate();
            }
            else
            {
                _mainPlatform.Deactivate();
            }
        }
        
        private void OnValidate()
        {
            if (_mainPlatform == null)
            {
                _mainPlatform = GetComponentInParent<MainPlatform>();
            }
        }
        
        [ProButton]
        private void Reset()
        {
            _mainPlatform = GetComponentInParent<MainPlatform>();
        }
    }
}