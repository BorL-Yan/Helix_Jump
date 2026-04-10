using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Platform.Multy
{
    
    public class MultyPlatformActivate : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        
        private Sequence sequence;
        public void Activate()
        {
             sequence = DOTween.Sequence();
            
             sequence.Append(transform.DOScale(Vector3.one*1.1f, 0.2f)).SetEase(Ease.OutCubic)
                .Append(transform.DOScale(Vector3.one, 0.3f)).SetEase(Ease.InCubic);
        }

        public void ActivateText()
        {
            if (_text == null) return;
            Color currentColor = _text.color;
            Color doColor = Color.white;
            
            sequence?.Kill();
            sequence = DOTween.Sequence();

            sequence.Append(_text.DOColor(doColor, 0.2f))
                .Append(_text.DOColor(currentColor, 0.2f))
                .SetLoops(3, LoopType.Restart);
        }


        public void Deactivate()
        {
            sequence?.Kill();
            transform.localScale = Vector3.zero;
        }
    }
}