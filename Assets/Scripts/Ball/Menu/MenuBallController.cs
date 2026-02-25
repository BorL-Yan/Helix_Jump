using Ball.Controller;
using DG.Tweening;
using Platform.Main_Scene;
using UnityEngine;
using VContainer;


public class MenuBallController : MonoBehaviour
{
    private GameAction _gameAction;
    private BallAction _ballAction;
    private SelectingPlatform _platform;
    
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private BallGroundedEffectMenu _effect;
    
    
    [SerializeField] private float JumpHeight = 4f;
    [SerializeField, Min(0.0001f)] private float TimeToApex = 0.4f;
    [SerializeField] private float FallMultiplier = 2f;
    [SerializeField] private float MaxFallSpeed = 20f;
    [Header("Hang Time Settings")]
    [SerializeField] private float HangThreshold = 0.5f; 
    [SerializeField] private float HangGravityMultiplier = 0.1f;
    
    [SerializeField] private float _moveSpeed;
    
    private float gravity;
    private float initialJumpVelocity;

    private bool _activateGravity;
    
    private Sequence moveSequence;
    
    [Inject]
    public void Construct(GameAction gameAction, BallAction ballAction)
    {
        _gameAction = gameAction;
        if (ballAction!= null)
        {
            _ballAction = ballAction;
        }else Debug.Log("Ball Action is null");
    }
    private void Awake()
    {
        _gameAction.SelectingPlatform += SelectPlatform;
        _gameAction.MoveToPlatform += SelectPlatform;
        _gameAction.ActivateGameSelectingPlatform += ActivateGame;
    }
    
    private void OnDestroy()
    {
        if (_gameAction != null)
        {
            _gameAction.SelectingPlatform -= SelectPlatform;
            _gameAction.MoveToPlatform -= SelectPlatform;
            _gameAction.ActivateGameSelectingPlatform -= ActivateGame;
        }
    }

    private void ActivateGame(SelectingPlatform platform)
    {
        _platform = platform;
        SetPosition();
        _rb.useGravity = false;
        _activateGravity = true;
        CalculateJumpParameters();
    }

    private void SelectPlatform(SelectingPlatform platform)
    {
        _platform = platform;
        _rb.linearVelocity = Vector3.zero;
        MoveToPlatform();
    } 

    private void SetPosition()
    {
        transform.position = _platform.transform.position + Vector3.up * JumpHeight;
    }

    private void MoveToPlatform()
    {
        _effect.Deactivate();
        _rb.linearVelocity = Vector3.zero;
        Vector3 targetPos =  _platform.transform.position + Vector3.up * JumpHeight;
        _activateGravity = false;
        
        float distance = Mathf.Abs(transform.position.z - targetPos.z);
        float moveTime = distance / _moveSpeed;
        
        moveSequence?.Kill();
        moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOMove(targetPos, moveTime))
            .OnComplete(() =>
            {
                _rb.linearVelocity = Vector3.zero;
                _activateGravity = true;
            });
    }
    
    public void FixedUpdate()
    {
        if (!_activateGravity)
        {
            return;
        }
    
        ApplyCustomGravity();
    }
    
    private void ApplyCustomGravity()
    {
        float currentGravity = gravity;
    
        if (Mathf.Abs(_rb.linearVelocity.y) < HangThreshold)
        {
            currentGravity *= HangGravityMultiplier;
        }else if (_rb.linearVelocity.y < 0)
        {
            currentGravity *= FallMultiplier;
        }
    
        _rb.AddForce(Vector3.up * currentGravity, ForceMode.Acceleration);

        if (_rb.linearVelocity.y < -MaxFallSpeed)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, -MaxFallSpeed, _rb.linearVelocity.z);
        }
    }

    public void Jump()
    {
        CalculateJumpParameters();
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, initialJumpVelocity, _rb.linearVelocity.z);
        
        _effect.Activate(_platform.transform.position);
        
        _platform.PlatformJumping();
        _ballAction.Jump?.Invoke();
    }
    
    
    private void CalculateJumpParameters()
    {
        initialJumpVelocity = (2 * JumpHeight) / TimeToApex;
        gravity = -initialJumpVelocity / TimeToApex;
    }
}
