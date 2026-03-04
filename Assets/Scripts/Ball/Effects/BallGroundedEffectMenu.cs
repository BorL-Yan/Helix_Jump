using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ball.Controller
{
    public class BallGroundedEffectMenu : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _effect;
        [SerializeField] private SpriteRenderer _circle;
        private float _circleScale;
        private float _circleAlpha;
        
        private Color _color;
        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private float _circleFadeOutDuration;
        private Sequence _sequence;

        private void Start()
        {
            _color = _effect.color;
            _effect.color = new Color(_color.r, _color.g, _color.b, 0);
            _circleScale = _circle.transform.localScale.x;
            _circle.transform.localScale = Vector3.zero;
            _circleAlpha = _circle.color.a;
            _circle.color = new Color(_circle.color.r, _circle.color.g, _circle.color.b, 0);
        }

        public void Activate(Vector3 pos)
        {
            transform.position = pos + Vector3.up * 0.12f;
            _circle.transform.position = pos + Vector3.up * 0.12f;
            
            transform.rotation = Quaternion.Euler(new Vector3
                (90,Random.Range(-180f, 180f),0));
            
            _sequence.Kill(true);
            
            _effect.color = _color;
            _circle.color = new Color(_circle.color.r, _circle.color.g, _circle.color.b,_circleAlpha);
            _sequence = DOTween.Sequence();

            _sequence.Append(_effect.DOColor(new Color(_color.r, _color.g, _color.b, 0), _fadeOutDuration).SetEase(Ease.InCubic))
                .Join(_circle.DOColor(new Color(_circle.color.r, _circle.color.g, _circle.color.b, 0), _circleFadeOutDuration).SetEase(Ease.InCubic))
                .Join(_circle.transform.DOScale(_circleScale, _circleFadeOutDuration))
                .OnComplete(() =>
                {
                    _effect.color = new Color(_color.r, _color.g, _color.b, 0);
                    _circle.transform.localScale = Vector3.zero;
                });
        }

        public void Deactivate()
        {
            _sequence.Kill(true);
        }
    }
}