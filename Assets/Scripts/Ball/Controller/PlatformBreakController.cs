using System;
using Ball.Configuration;
using DG.Tweening;
using Level.Controllers;
using UnityEngine;
using VContainer;

namespace Ball.Controller
{
    public class PlatformBreakController : MonoBehaviour
    {
        [SerializeField] private float _distance;
        [SerializeField] private Transform _ball;
        private float _speed;

        private Collider _collider;
        
        private BallAction _ballAction;
        private BallFlags _flags;
        private LevelAction _levelAction;
        private BallConfig _ballConfig;
        
        private Material _ballMaterial;
        
        [Inject]
        public void Construct(BallAction ballAction, BallFlags flags, BallConfig ballConfig, LevelAction levelAction)
        {
            _ballAction = ballAction;
            _flags = flags;
            _ballConfig = ballConfig;
            _levelAction = levelAction;
        }

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _speed = 0.7f;
            _ballMaterial = _ballConfig.GetMaterial().BallMaterial;
        }


        private void Activate(Vector3 position)
        {
            _collider.enabled = true;
            transform.position = _ball.position;
            Sequence move = DOTween.Sequence();

            move.Append(transform.DOMoveY(transform.position.y - _distance, _speed).SetEase(Ease.Linear)
                    .OnUpdate(() =>
                    {
                        RaycastHit[] results = new RaycastHit[4];
                        var size = Physics.RaycastNonAlloc(transform.position + Vector3.up, Vector3.down, results, 1.4f);
                        for (int i = 0; i < size; i++)
                        {
                            var hit = results[i];
                            if (hit.transform.CompareTag("BreakPlatform"))
                            {
                                Break(hit.transform.GetInstanceID());
                            }
                        }
                    }))
                .OnComplete(()=>
                {
                    _collider.enabled = false;
                });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BreakPlatform"))
            {
                Break(other.transform.GetInstanceID());
            }
        }

        private void Break(int id)
        {
            var platform = PlatformActivatorList.Instance.GetPlatformActivate(id);
            platform.ActivateBoom(_ballMaterial);
            
            
            _flags.currentPlatformBreak++;     
            if(_flags.currentPlatformBreak >= _ballConfig.MaxPlatformBreak - 1)
            {
                ComboTrail.Instance.Activate();
            }
            _levelAction.OnSetPoint?.Invoke(_flags.currentPlatformBreak);
        }

        private void OnEnable()
        {
            _ballAction.ActivateBreakPlatform += Activate;
        }

        private void OnDisable()
        {
            _ballAction.ActivateBreakPlatform -= Activate;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position +Vector3.down * _distance);
        }
    }
}