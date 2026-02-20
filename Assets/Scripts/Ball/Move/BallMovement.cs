
using System;
using UnityEngine;
using VContainer.Unity;

public class BallMovement : IStartable, IDisposable
{
    private readonly Transform _ball;
    private readonly BallAction _ballAction;


    private void MoveTo(Vector3 position)
    {
        _ball.position = position;
    }
    
    
    public void Start()
    {
        
    }
    
    public void Dispose()
    {
        
    }
}
