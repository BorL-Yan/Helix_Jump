using DG.Tweening;
using UnityEngine;

namespace Platform.Multy
{
    public class MultyPlatformActivate : MonoBehaviour
    {
        private Sequence sequence;
        public void Activate()
        {
             sequence = DOTween.Sequence();
            
             sequence.Append(transform.DOScale(Vector3.one*1.1f, 0.2f)).SetEase(Ease.OutCubic)
                .Append(transform.DOScale(Vector3.one, 0.3f)).SetEase(Ease.InCubic);
        }

        public void Deactivate()
        {
            sequence?.Kill();
            transform.localScale = Vector3.zero;
        }
    }
}