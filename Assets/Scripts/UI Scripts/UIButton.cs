using System;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public abstract class UIButton : MonoBehaviour, IPointerClickHandler
{
    protected float _clickScale = 0f;
    protected float _clickDureation = 0f;
    protected Sequence anim;
    public event Action OnClick;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        ButtonAnimation();
    }

    protected virtual void ButtonAnimation()
    {
        Click();
        OnClick?.Invoke();
        /*anim?.Kill();
        anim = DOTween.Sequence();
        anim.Append(transform.DOScale(Vector3.one*_clickScale, _clickDureation))
            .AppendCallback(() =>
            {
                Click();
                OnClick?.Invoke();
            })
            .Append(transform.DOScale(Vector3.one, _clickDureation));*/
    }
    
    protected abstract void Click();
    
    /*private void OnDestroy()
    {
        anim?.Kill();
    }*/
}