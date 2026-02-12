using System;
using Ball.Configuration;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BallJump : IStartable, IDisposable, IFixedTickable
{
    private readonly BallAction _action;
    private readonly BallConfig _config;
    
    private Rigidbody _rb;
    
    private float gravity { get; set; }
    private float initialJumpVelocity { get; set; }
    
    [Inject]
    public BallJump( BallAction action, BallConfig config, Rigidbody rb)
    {
        _action = action;
        _config = config;
        _rb = rb;
    }
    
    public void Jump()
    {
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, initialJumpVelocity, _rb.linearVelocity.z);
        CalculateJumpParameters();
    }

    public void FixedTick()
    {
        ApplyCustomGravity();
    }

    private void ApplyCustomGravity()
    {
        float currentGravity = gravity;
        
        if (_rb.linearVelocity.y < 0)
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
    
    public void Start()
    {
        CalculateJumpParameters();
        _action.Jump += Jump;
        _rb.useGravity = false;
    }
    
    public void Dispose()
    {
        _action.Jump -= Jump;
    }
}
