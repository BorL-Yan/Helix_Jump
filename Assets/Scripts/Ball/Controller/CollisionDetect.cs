using Ball.Configuration;
using Level.Controllers;
using Platform.Multy;
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
        private Material _ballMaterial;
        private bool _activated;
        
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
            _ballMaterial = _ballConfig.GetMaterial().BallMaterial;
            ComboTrail.Instance.SetMaterial(_ballMaterial);
        }

        
        
        private void OnTriggerEnter(Collider collider)
        {
            if(!_activated) return;
            _activated = false;
            Invoke("DiactivateCollision", 0.02f);
            
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
                case "EnemyPlatform":
                {
                    LoosePlatform();
                    break;
                }
                case "FinishPlatform":
                {
                    FinishPlatform(collider.transform);
                    break;
                }
                case "MultiplyPlatform":
                {
                    MultyplyPlatform(collider.transform);
                    break;
                }
                case "MultiplyEmptyPlatform":
                {
                    MultiplyEmptyPlatform(collider.transform);
                    break;
                }
                case "MultiplyFinishPlatform":
                {
                    //TODO final effect
                    
                    MultyplyPlatform(collider.transform);
                    break;
                }
                case "Key":
                {
                    TakeKey();
                    break;
                }
                case "BreakPlatformObject":
                {
                    collider.gameObject.SetActive(false);
                    _ballAction.ActivateBreakPlatform(collider.transform.position);
                    break;
                }
                
            }
        }

        private void Platform(Transform platformPos)
        {
            bool big = false;
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
                            platform.ActivateBoom(_ballMaterial);
                            _levelAction.OnSetPoint?.Invoke(_flags.currentPlatformBreak);
                            _levelAction.OnPointAnimation?.Invoke(_flags.currentPlatformBreak);
                            big = true;
                        }
                    }
                }
            }
            
            _flags.isGround = true;
            _ballAction?.Jump();
            BallGroundedEffect.Instance.GroundedEffect(platformPos,big);

            ComboTrail.Instance.Deactivate();
            if (_flags.activeEffect)
            {
                _flags.activeEffect = false;
                _ballAction.ActivateCombo?.Invoke(false);
            }
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
                if (!_flags.activeEffect)
                {
                    _flags.activeEffect = true;
                    _ballAction.ActivateCombo?.Invoke(true);
                }
            }
            
            if (_flags.currentPlatformBreak > 6)
            {
                // TODO Activate new Star skin, 
                var settings = GameSave.GetSettings();
                if (!settings.ActiveSkins.TryGetValue(BallSkinType.Star, out var value))
                {
                    GameManager.Instance.Action.OnActivateNewSkin?.Invoke(BallSkinType.Star);
                    settings.ActiveSkins.TryAdd(BallSkinType.Star, true);
                    GameSave.SetSettings(settings);
                }
            }
            
            _levelAction.OnSetPoint?.Invoke(_flags.currentPlatformBreak);
        }

        private void LoosePlatform()
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
                            platform.ActivateBoom(_ballMaterial);
                            _levelAction.OnSetPoint?.Invoke(_flags.currentPlatformBreak);
                            return;
                        }
                    }
                }
            }
            
            _flags.gravity = false;
            _levelAction.OnLoos?.Invoke();
            _levelAction.OnEndLevel?.Invoke();
            _ballAction.DeactivateGravity?.Invoke();
        }

        private void TakeKey()
        {
            _levelAction.OnTakeKey();
            Debug.Log("Take Key");
            
        }
        
        private void FinishPlatform(Transform platformTransform)
        {
            _ballAction?.Jump();
            BallGroundedEffect.Instance.GroundedEffect(platformTransform, false);
            ComboTrail.Instance.Deactivate();
            _ballAction.ActivateCombo?.Invoke(false);
            _levelAction.OnFinishLevel?.Invoke();
            _ballAction?.Jump();
            
            _levelAction.OnFinishEffectActivate?.Invoke(transform.position);
            _flags.currentPlatformBreak = 0;
            _flags.activeEffect = false;

        }

        private void MultyplyPlatform(Transform platformTransform)
        {
            _levelAction.OnActivateFinalPointAnimation();
            _levelAction.OnEndLevel?.Invoke();
            _ballAction.DeactivateGravity?.Invoke();
            _flags.gravity = false;
            
            _ballAction.ActivateCombo?.Invoke(false);
            
            _levelAction.OnFinishEffectActivate?.Invoke(transform.position);
            Debug.Log(platformTransform.name);
            platformTransform.GetComponent<MultyPlatformDetect>().Activate();

        }

        private void MultiplyEmptyPlatform(Transform platformTransform)
        {
            _levelAction.AddMultiplyCount?.Invoke();
            _flags.currentPlatformBreak++;
            if (_flags.currentPlatformBreak >= _ballConfig.MaxPlatformBreak - 1 && !_flags.activeEffect)
            {
                _flags.activeEffect = true;
                _ballAction.ActivateCombo?.Invoke(true);
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