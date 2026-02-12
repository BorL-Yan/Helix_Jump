using System;
using Ball.Configuration;
using Ball.Controller;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BallLifeTimeScope : LifetimeScope
{
    [SerializeField] private Rigidbody _rb; 
    [SerializeField] private Transform _header;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_rb);
        
        builder.RegisterInstance(_header);
        
        builder.Register<BallAction>(Lifetime.Singleton);
        
        
        builder.Register<BallJump>(Lifetime.Singleton)
            .AsSelf()
            .As<IStartable, IDisposable, IFixedTickable>();
        builder.Register<BallFlags>(Lifetime.Singleton);
        
        
        BallGroundedEffect.Instance.Initialize(_header);
        
    }
}
