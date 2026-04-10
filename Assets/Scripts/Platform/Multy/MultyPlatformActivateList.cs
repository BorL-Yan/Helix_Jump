using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using TMPro;
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
        [VInspector.Button]
        public void ActivateDetect()
        {
            foreach (var item in _platforms)
            {
                item.ActivateText();
            }
        }
        
        private void OnValidate()
        {
            UpdatePlatformList();
        }
        
        [VInspector.Button]
        private void Reset()
        {
            UpdatePlatformList();
        }
        [VInspector.Button]
        public void SetMaterial(Material material)
        {
            var items = GetComponentsInChildren<MeshRenderer>(true);
            foreach (var item in items)
            {
                item.material = material;
            }
        }

        [VInspector.Button]
        public void SetText(string value)
        {
            var items = GetComponentsInChildren<TMP_Text>(true);
            foreach (var item in items)
            {
                item.text = value;
            }
        }
        
        private void UpdatePlatformList()
        {
            var children = GetComponentsInChildren<MultyPlatformActivate>(true);
            _platforms = new List<MultyPlatformActivate>(children);
        }
        
    }
}