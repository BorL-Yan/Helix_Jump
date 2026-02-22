using System;
using Ball.Configuration;
using DG.Tweening;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BallJump : IStartable, IDisposable, IFixedTickable
{
    private readonly BallAction _action;
    private readonly BallConfig _config;

    private readonly LevelAction _levelAction;
    private readonly BallFlags _flags;
    private Rigidbody _rb;
    
    private float gravity { get; set; }
    private float initialJumpVelocity { get; set; }
    
    [Inject]
    public BallJump( BallAction action, BallConfig config, Rigidbody rb, BallFlags flags, LevelAction levelAction)
    {
        _action = action;
        _config = config;
        _rb = rb;
        _flags = flags;
        _levelAction = levelAction;
    }
    
    public void Jump()
    {
        CalculateJumpParameters();
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, initialJumpVelocity, _rb.linearVelocity.z);
    }

    public void FixedTick()
    {
        if (!_flags.gravity)
        {
            return;
        }
        
        ApplyCustomGravity();
    }

    private void ApplyCustomGravity()
    {
        float currentGravity = gravity;
        
        if (Mathf.Abs(_rb.linearVelocity.y) < _config.HangThreshold)
        {
            currentGravity *= _config.HangGravityMultiplier;
        }else if (_rb.linearVelocity.y < 0)
        {
            currentGravity *= _config.FallMultiplier;
        }
        
        _rb.AddForce(Vector3.up * currentGravity, ForceMode.Acceleration);

        if (_rb.linearVelocity.y < -_config.MaxFallSpeed)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, -_config.MaxFallSpeed, _rb.linearVelocity.z);
        }
    }
    
    private void CalculateJumpParameters()
    {
        gravity = -(2 * _config.JumpHeight) / Mathf.Pow(_config.TimeToApex, 2);
        initialJumpVelocity = (2 * _config.JumpHeight) / _config.TimeToApex;
    }

    private void EndGravity()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.DOMoveY(_rb.position.y + 0.1f, 0.01f);
    }
    
    
    public void Start()
    {
        CalculateJumpParameters();
        _action.Jump += Jump;
        
        _rb.useGravity = false;

        _action.DeactivateGravity += EndGravity;
    }
    
    public void Dispose()
    {
        _action.Jump -= Jump;
        _action.DeactivateGravity -= EndGravity;
    }
}
