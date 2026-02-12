using Ball.Configuration;
using Level.Controllers;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Ball.Controller
{
    public class CollisionDetect : MonoBehaviour
    {
        [SerializeField] private LayerMask _detectlayerMask;
        [SerializeField] private Vector3 _detectionSize;

        [SerializeField] private float _breakPlatformDistance;
        
        
        private BallAction _ballAction;
        private BallFlags _flags;
        private BallConfig _ballConfig;
        private LevelAction _levelAction;
        private bool _activated;
        private Material _ballmaterial;
        
        [Inject]
        public void Construct(BallAction ballAction, BallFlags flags, BallConfig ballConfig, LevelAction levelAction)
        {
            _ballAction = ballAction;
            _flags = flags;
            _ballConfig = ballConfig;
            _levelAction = levelAction;
        }

        private void Start()
        {
            _activated = true;
            _ballmaterial = _ballConfig.GetMaterial();
            ComboTrail.Instance.SetMaterial(_ballConfig.GetMaterial());
        }

        
        
        private void OnTriggerEnter(Collider collider)
        {
            if(!_activated) return;
            _activated = false;
            Invoke("DiactivateCollision", 0.1f);
            
            switch (collider.transform.tag)
            {
                case "BreakPlatform":
                {
                    BreakPlatform(collider.transform);
                    break;
                }
                case "Platform":
                {
                    Platform(collider.transform);
                    break;
                }
                case "FinishPlatform":
                {
                    _levelAction.OnFinishLevel?.Invoke();
                    _ballAction?.Jump();
                    break;
                }
                case "MultiplyPlatform":
                {
                    break;
                }
                case "EnemyPlatform":
                {
                    _levelAction.OnLoos?.Invoke();
                    break;
                }
            }
        }

        private void Platform(Transform platformPos)
        {
            if (_flags.currentPlatformBreak >= _ballConfig.MaxPlatformBreak)
            {
                RaycastHit[] results = new RaycastHit[5];
                var size = Physics.RaycastNonAlloc(transform.position + Vector3.up, Vector3.down, results, _breakPlatformDistance+1, _detectlayerMask);
                for (int i = 0; i < size; i++)
                {
                    var hit = results[i];
                     
                    if (hit.transform.CompareTag("BreakPlatform"))
                    {
                        var platform = PlatformActivatorList.Instance.GetPlatformActivate(hit.transform.GetInstanceID());
                        if (platform != null)
                        {
                            platform.ActivateBoom(_ballmaterial);
                        }
                    }
                }
                
            }
            
            _flags.isGround = true;
            _ballAction?.Jump();
            BallGroundedEffect.Instance.GroundedEffect(platformPos);

            ComboTrail.Instance.Deactivate();
            _flags.currentPlatformBreak = 0;
        }

        private void BreakPlatform(Transform platformTransform)
        {
            PlatformActivatorList.Instance.GetPlatformActivate(platformTransform.GetInstanceID())
                .ActivateBoom();
            
            _flags.currentPlatformBreak++;     
            
            if(_flags.currentPlatformBreak >= _ballConfig.MaxPlatformBreak - 1)
            {
                ComboTrail.Instance.Activate();
            }
            
        }
        
        private void DiactivateCollision()
        {
            _activated = true;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.down * _breakPlatformDistance);
        }
    }
}