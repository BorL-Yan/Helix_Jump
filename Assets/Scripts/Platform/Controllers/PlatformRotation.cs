using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlatformRotation : IStartable, IDisposable
{ 
    private readonly GameAction _gameAction;
    private readonly Transform _header;
    private readonly BallConfig _ballConfig;
    private readonly LevelAction _levelAction;
    
    private bool _activeRotation = true;
    private bool _firstInput = false;
    
    [Inject]
    public PlatformRotation(GameAction gameAction, Transform header, BallConfig ballConfig, LevelAction levelAction)
    {
        _gameAction = gameAction;
        _header = header;
        _ballConfig = ballConfig;
        _levelAction = levelAction;
    }
    
    public void Rotation(float direction)
    {
        if(!_activeRotation) return;
        if (!_firstInput)
        {
            _firstInput = true;
            GameManager.Instance.Action.ActivateGlobalPanel(false);
        }
        
        Vector3 rotation = _header.eulerAngles;
        float targetYAngle = direction * _ballConfig.RotationSpeed;
        rotation.y -= targetYAngle;
        
        Quaternion targetRotation = Quaternion.Euler(rotation);
        _header.localRotation = targetRotation;
    }

    private void ActivateLevel()
    {
        Debug.Log("ActivateLevel");
        _activeRotation = true;
    }

    private void DeactivateLevel()
    {
        _activeRotation = false;
    }
    
    
    public void Start()
    {
        _gameAction.MoveX += Rotation;
        _levelAction.OnStartLevel += ActivateLevel;
        _levelAction.OnEndLevel += DeactivateLevel;
    }
    
    public void Dispose()
    {
        _gameAction.MoveX -= Rotation;
        _levelAction.OnStartLevel -= ActivateLevel;
        _levelAction.OnEndLevel -= DeactivateLevel;
    }
}
