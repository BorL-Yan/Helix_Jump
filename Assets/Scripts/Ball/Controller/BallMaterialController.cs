using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using Level.Controllers;
using UnityEngine;
using VContainer;

namespace Ball.Controller
{
    public class BallMaterialController :  MonoBehaviour
    {
        [SerializeField] private List<BallMaterial> _materials;

        [Inject]
        public void Construct(BallConfig config)
        {
            Material mat = config.GetMaterial();
            foreach (var material in _materials)
            {
                material.SetMaterial(mat);
            }
        }
        
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
        
    }
}