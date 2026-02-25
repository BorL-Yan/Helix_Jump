using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Serialization;

public abstract class UIButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected float _clickScale = 1.2f;
    [SerializeField] protected float _clickDuration = 0.125f;
    protected Sequence anim;
    public event Action OnClick;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        ButtonAnimation();
    }
    
    protected virtual void ButtonAnimation()
    {
        anim?.Kill();
        anim = DOTween.Sequence();
        anim.Append(transform.DOScale(Vector3.one *_clickScale, _clickDuration))
            .AppendCallback(() =>
            {
                Click();
                OnClick?.Invoke();
            })
            .Append(transform.DOScale(Vector3.one, _clickDuration));
    }
    
    protected abstract void Click();
    
    private void OnDestroy()
    {
        anim?.Kill();
    }
}