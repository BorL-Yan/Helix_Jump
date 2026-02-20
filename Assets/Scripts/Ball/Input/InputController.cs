using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using VContainer;
using VContainer.Unity;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class InputController: IStartable, IDisposable
{
    private readonly PlayerInput _input;
    private readonly GameAction _gameAction;
    
    [Inject]
    public InputController(GameAction action)
    {
        _gameAction = action ?? throw new ArgumentNullException(nameof(action), "BallAction не был внедрен!");
        _input = new();
    }
    
    public void Init()
    {
        _input.Enable();
        
        _input.Screan.Touch.performed += e => Touch_0(e.ReadValue<TouchState>());
        _input.Screan.Mouse.started += e => ButtonDown(e);
        
    }

    private void Touch_0(TouchState touch)
    {
        if (touch.phase == TouchPhase.Moved)
        {
            _gameAction.MoveX?.Invoke(touch.delta.x);
            _gameAction.MoveY?.Invoke(touch.delta.y);
        }
        
    }

    private void ButtonDown(InputAction.CallbackContext c)
    {
        Vector2 screenPosition = Pointer.current.position.ReadValue();
        _gameAction.TouchScreen?.Invoke(screenPosition);
    }
    
    public void Start()
    {
        Init();
    }
   
    public void Dispose()
    {
        _input?.Dispose();
    }
}
