using TMPro;
using UnityEngine;

namespace Platform.Main_Scene
{
    public class MainPlatform : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material _activeMaterial;
        [SerializeField] private Material _selectMaterial;
        [SerializeField] private Material _deactiveMaterial;

        [SerializeField] private SelectingPlatform _selectorPlatform;
        [SerializeField] private TMP_Text _text;
        
        private void Awake()
        {
            _text.text = _selectorPlatform.platformID.ToString();
        }
        
        
        public void Activate()
        {
            _meshRenderer.material = _activeMaterial;    
            _selectorPlatform.gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _meshRenderer.material = _deactiveMaterial;
            _selectorPlatform.gameObject.SetActive(false);
        }

        public void Select()
        {
            _meshRenderer.material = _selectMaterial;
        }
    }
}