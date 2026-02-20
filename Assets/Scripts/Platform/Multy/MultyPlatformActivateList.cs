using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platform.Multy
{
    public class MultyPlatformActivateList : MonoBehaviour
    {
        [SerializeField] public List<MultyPlatformActivate> _platforms;

        public void Activate()
        {
            foreach (var platform in _platforms)
                platform.Activate();
        }

        public void Deactivate()
        {
            foreach (var platform in _platforms)
                platform.Deactivate();
        }
        
        
        private void OnValidate()
        {
            UpdatePlatformList();
        }
        
        [ProButton]
        private void Reset()
        {
            UpdatePlatformList();
        }

        private void UpdatePlatformList()
        {
            var children = GetComponentsInChildren<MultyPlatformActivate>(true);
            _platforms = new List<MultyPlatformActivate>(children);
        }
        
    }
}