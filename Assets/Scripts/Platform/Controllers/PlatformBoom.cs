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
        Vector3 startLocalPos = transform.localPosition;
        Vector3 startLocalRot = transform.localEulerAngles;
        
        Vector3 direction = transform.right;
        
        sequence
            // 1. Двигаем локально по X (относительно родителя)
            .Append(transform.DOMove(direction * 2f, 0.5f)).SetEase(Ease.InOutCubic)
            .Join(transform.DOLocalMoveY(-10, 2).SetLink(gameObject))
            .SetEase(Ease.InOutCubic)
            .Join(transform.DOLocalRotate(new Vector3( Random.Range(-20, 20), startLocalRot.y, 30), 0.5f)).SetEase(Ease.InOutCubic)
            //
            .OnComplete(() =>
            {
                // Возвращаем в исходное локальное состояние
                // transform.localPosition = startLocalPos;
                // transform.localRotation = Quaternion.Euler(startLocalRot);
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