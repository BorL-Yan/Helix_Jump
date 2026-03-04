using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;


namespace Ball.Controller
{
    public class BallMaterialController :  MonoBehaviour
    {
        [SerializeField] private List<BallMaterial> _materials;
        
        private void OnValidate()
        {
            UpdateMaterialList();
        }
        
        [ProButton]
        private void Reset()
        {
            UpdateMaterialList();
        }

        private void UpdateMaterialList()
        {
            var children = GetComponentsInChildren<BallMaterial>(true);
            _materials = new List<BallMaterial>(children);
        }

        public void SetMaterial(Material newMaterial)
        {
            if (newMaterial == null)
            {
                Debug.LogWarning("[BallMaterialController] Cannot set null material");
                return;
            }
            
            foreach (var material in _materials)
            {
                material.SetMaterial(newMaterial);
            }
        }
        
    }
}