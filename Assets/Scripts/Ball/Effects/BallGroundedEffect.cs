using DG.Tweening;
using Level.Controllers;
using UnityEngine;

namespace Ball.Controller
{
    public class BallGroundedEffect : SingletonScene<BallGroundedEffect>
    {
        [SerializeField] private SpriteRenderer _effectMat;
        [SerializeField] private ParticleSystem _particleSystem;

        private Color _color;
        [SerializeField] private float _fadeOutDuration;
        private Sequence _sequence;

        private Transform _ball;

        public void Initialize(Transform ball)
        {
            _ball = ball;
            _particleSystem.Clear();
        }
        
        private void Start()
        {
            _particleSystem.transform.SetParent(PlatformActivatorList.Instance.transform);
            _particleSystem.transform.localScale = Vector3.one;
            transform.SetParent(null);
            _color = _effectMat.color;
            _effectMat.color = new Color(0,0,0,0);
        }

        public void GroundedEffect(Transform hit)
        {
            transform.SetParent(hit.transform.parent);

            transform.position = new Vector3(
                _ball.position.x,
                hit.position.y + 0.12f,
                _ball.position.z);
            _particleSystem.transform.position = transform.position + Vector3.down * 0.2f;
            
            transform.rotation = Quaternion.Euler(new Vector3
                (90,Random.Range(-180f, 180f),0));
            
            _sequence.Kill(true);
            
            _effectMat.color = _color;
            _particleSystem.Clear();
            _particleSystem.Play();
            
            _sequence = DOTween.Sequence();

            _sequence.Append(_effectMat.DOColor(new Color(_color.r, _color.g, _color.b, 0), _fadeOutDuration))
                .OnComplete(() => _effectMat.color = new Color(_color.r, _color.g, _color.b, 0));
        }
    }
}