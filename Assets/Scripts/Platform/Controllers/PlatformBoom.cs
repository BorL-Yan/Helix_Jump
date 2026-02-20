using System;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformBoom : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private MeshRenderer _meshRenderer;
    
    Sequence sequence;
    
    public void SetMaterial(Material material)
    {
        if (_meshRenderer != null && material != null)
        {
            _meshRenderer.material = material;
        }
    }
    
    [ProButton]
    public void BoomMethod()
    {
        _collider.enabled = false;
        sequence = DOTween.Sequence();
        Vector3 startLocalRot = transform.localEulerAngles;
        
        Vector3 direction = transform.right;
        
        sequence
            .Join(transform.DOMove(direction * 2f, 0.5f)).SetEase(Ease.InQuad)
            .Join(transform.DOLocalMoveY(-10, 2).SetLink(gameObject))
                .SetEase(Ease.InQuad)
            .Join(transform.DOLocalRotate(new Vector3(Random.Range(-20, 20), startLocalRot.y, 30), 0.5f))
                .SetEase(Ease.InQuad)
            .InsertCallback(0.8f, () =>
            {
                gameObject.SetActive(false);
            });
    }

    private void OnEnable()
    {
        sequence.Kill();
    }

    private void OnDestroy()
    {
        sequence.Kill();
    }
}