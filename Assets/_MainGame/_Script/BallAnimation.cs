using DG.Tweening;
using UnityEngine;

public class BallAnimation 
{
    private float _jumpDuration;

    private Vector3 startScale;

    public BallAnimation(Transform _transform, float jumpDuration)
    {
        startScale = _transform.localScale;
        _jumpDuration = jumpDuration;
    }
    /*
    public void Jump(Transform _transform)
    {
        Vector3 worldScale = _transform.lossyScale;
        _transform.localScale = Vector3.one; // Сброс локального масштаба
        _transform.DOScale(worldScale * 2f, duration).SetEase(Ease.OutBack);
    }
    */    
    public void Jump(Transform _transform)
    {
        _transform.DOScale(new Vector3( startScale.x * 1.2f,startScale.y * 0.8f,startScale.z * 1.2f), _jumpDuration / 2).SetEase(Ease.OutBounce).OnComplete(() => {
            _transform.DOScale(new Vector3(startScale.x * 0.8f, startScale.y * 1.2f, startScale.z * 0.8f
            ), _jumpDuration)
            .SetEase(Ease.InOutBounce)
            .OnComplete(() =>
            {
                _transform.DOScale(startScale, 0.1f);
            });
        });
    }
}
