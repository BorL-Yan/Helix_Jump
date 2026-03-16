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
        [SerializeField] private Material _comboMaterial;
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

        public void SetMaterial(Material newMaterial)
        {
            _defaultMaterial = newMaterial;
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
                _trailRenderer.material = _comboMaterial;
            }
            else
            {
                foreach (var material in _materials)
                {
                    material.SetMaterial(_defaultMaterial);
                }
                _trailRenderer.material = _defaultMaterial;
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