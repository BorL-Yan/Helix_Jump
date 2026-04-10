using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

namespace Level.Controllers
{
    public class PlatformController : MonoBehaviour
    {
        [SerializeField] private List<PlatformBoom> _platforms;

        [ProButton]
        public void ActivateBoom(Material material = null)
        {
            foreach (var item in _platforms)
            {
                item.BoomMethod();
                if(material != null )item.SetMaterial(material);
            }
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
            var children = GetComponentsInChildren<PlatformBoom>(true);
            _platforms = new List<PlatformBoom>(children);
            foreach (var VARIABLE in children)
            {
                
            }
        }
        
    }
}