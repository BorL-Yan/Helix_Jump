using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Ball.Controller
{
    public class BallGroundedEffect : SingletonScene<BallGroundedEffect>
    {
        [SerializeField] private SpriteRenderer _effectMat;
        [SerializeField] private ParticleSystem _particleSystem;

        [SerializeField] private Color _color;
        [SerializeField] private float _fadeOutDuration;
        private Sequence _sequence;

        private Transform _ball;

        public void Initialize(Transform ball)
        {
            _ball = ball;
            _particleSystem.Pause();
        }
        
        private void Start()
        {
            transform.SetParent(null);
            _effectMat.color = new Color(0,0,0,0);
        }

        public void GroundedEffect(Transform hit)
        {
            transform.SetParent(hit.transform);

            transform.position = new Vector3(
                _ball.position.x,
                hit.position.y + 0.12f,
                _ball.position.z);
            
            transform.rotation = Quaternion.Euler(new Vector3
                (90,Random.Range(-180f, 180f),0));
            
            _sequence.Kill(true);
            
            _effectMat.color = _color;
            _particleSystem.Play();
            
            _sequence = DOTween.Sequence();

            _sequence.Append(_effectMat.DOColor(new Color(_color.r, _color.g, _color.b, 0), _fadeOutDuration))
                .OnComplete(() => _effectMat.color = new Color(_color.r, _color.g, _color.b, 0));
        }
    }
}