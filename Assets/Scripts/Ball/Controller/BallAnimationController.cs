using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Ball.Controller
{
    public class BallAnimationController : MonoBehaviour
    {
        [SerializeField] private float _rotateDuration;
        [SerializeField] private float _scaleDuration;

        [SerializeField, Range(0, 1f)] private float _scaleRange;
        [SerializeField, MinMaxRange(0,360)] private Vector2Int _rotationRange;
        
        private Sequence _sequence;
        
        [SerializeField] private Transform _ball;

        private BallAction _ballAction;
        
        [Inject]
        public void Construct(BallAction ballAction)
        {
            _ballAction = ballAction;
        }

        private void Aniamtion()
        {
            _sequence.Kill(true);
            _sequence = DOTween.Sequence();
            Vector3 randomRotation = new Vector3();
            switch (Random.Range(1,4))
            {
                case 1:
                {
                    randomRotation = new Vector3(1, 0.3f, 0.2f);
                    break;
                }
                case 2:
                {
                    randomRotation = new Vector3(0.1f, 1, 0.4f);
                    break;
                }
                case 3:
                {
                    randomRotation = new Vector3(0.5f, 0.2f, 1);
                    break;
                }
            }

            randomRotation = _ball.eulerAngles + randomRotation * Random.Range(_rotationRange.x, _rotationRange.y);
             // rs = 0.1 => x = 0.9 => z = 1.1 
             
            float randomScale = Random.Range(1 - _scaleRange, 1 + _scaleRange);
            Vector3 scale = new Vector3(randomScale, 1, 2 - randomScale);
            _ball.localScale = scale;

            _sequence.Append(_ball.DORotate(randomRotation, _rotateDuration, RotateMode.FastBeyond360)).SetEase(Ease.OutCirc)
                .Join(_ball.DOScale(scale, _scaleDuration)).SetEase(Ease.OutQuint)
                .InsertCallback(_scaleDuration, () =>
                {
                    _ball.DOScale(Vector3.one, _scaleDuration * 2);
                })
                .OnComplete(() =>
                {
                    _ball.localScale = Vector3.one;
                });
        }
        
        private void Start()
        {
            _ballAction.Jump += Aniamtion;
        }
        
        private void OnDestroy()
        {
            _ballAction.Jump -= Aniamtion;
            _sequence.Kill(true);
        }
    }
}