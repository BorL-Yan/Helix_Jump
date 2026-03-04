using DG.Tweening;
using UnityEngine;

namespace UI_Scripts
{
    public class ButtonActivateAnimation : MonoBehaviour
    {
        private Sequence sequence;

        private void OnEnable()
        {
            BuyAnimation();
        }
        
        private void BuyAnimation()
        {
            sequence.Kill();
            sequence = DOTween.Sequence();
            transform.localScale = Vector3.one * 0.6f;
            sequence.Append(transform.DOScale(1.2f, 0.25f))
                .Append(transform.DOScale(1, 0.1f));
        }
    }
}