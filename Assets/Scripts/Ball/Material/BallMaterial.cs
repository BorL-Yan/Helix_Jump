using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;

public class BallMaterial : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    
    
    
    public void SetMaterial(Material newMaterial)
    {
        _meshRenderer.material = newMaterial;
    }

    private void OnValidate()
    {
        if (_meshRenderer == null)
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }
    }
    
    [ProButton]
    private void Reset()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
}
