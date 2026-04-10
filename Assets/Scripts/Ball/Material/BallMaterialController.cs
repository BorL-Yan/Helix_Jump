using System;
using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using VContainer;


namespace Ball.Controller
{
    public class BallMaterialController :  MonoBehaviour
    {
        [SerializeField] private List<BallMaterial> _materials;
        private Material _defaultMaterial;
        private Material _defaultTrailMaterial;
        [SerializeField] private Material _comboMaterial;
        [SerializeField] private Material _comboTrailMaterial;
        [SerializeField] private TrailRenderer _trailRenderer;

        private BallAction _ballAction;

        [Inject]
        public void Construct(BallAction action)
        {
            _ballAction = action;
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

        public void SetMaterial(Material newMaterial, Material trailMaterial)
        {
            _defaultMaterial = newMaterial;
            _defaultTrailMaterial = trailMaterial;
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

        private void ActivateCombo(bool value)
        {
            if (value)
            {
                foreach (var material in _materials)
                {
                    material.SetMaterial(_comboMaterial);
                }
                _trailRenderer.material = _comboTrailMaterial;
            }
            else
            {
                foreach (var material in _materials)
                {
                    material.SetMaterial(_defaultMaterial);
                }
                _trailRenderer.material = _defaultTrailMaterial;
            }
        }
        
        private void OnEnable()
        {
            if(_ballAction!= null)
                _ballAction.ActivateCombo += ActivateCombo;
        }

        private void OnDisable()
        {
            if(_ballAction!= null)
                _ballAction.ActivateCombo -= ActivateCombo;
        }
    }
}