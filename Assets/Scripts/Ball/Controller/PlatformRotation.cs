using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlatformRotation : IStartable, IDisposable
{ 
    private readonly GameAction _gameAction;
    private readonly Transform _header;
    private readonly BallConfig _ballConfig;
    
    [Inject]
    public PlatformRotation(GameAction gameAction, Transform header, BallConfig ballConfig)
    {
        _gameAction = gameAction;
        _header = header;
        _ballConfig = ballConfig;
    }
    
    public void Rotation(float direction)
    {
        Vector3 rotation = _header.eulerAngles;
        float targetYAngle = direction * _ballConfig.RotationSpeed;
        rotation.y -= targetYAngle;
        
        Quaternion targetRotation = Quaternion.Euler(rotation);
        _header.localRotation = targetRotation;
    }

    public void Start()
    {
        _gameAction.MoveX += Rotation;
    }
    
    public void Dispose()
    {
        _gameAction.MoveX -= Rotation;
    }
}
