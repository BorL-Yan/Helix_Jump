using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifeTimeScope : LifetimeScope
{
    [SerializeField] private BallConfig _config; 
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_config);
        
        builder.Register<GameAction>(Lifetime.Singleton);
        builder.Register<InputController>(Lifetime.Singleton)
            .AsSelf()
            .As<IStartable, IDisposable>();
        
        GameSave.Init();
    }    
}
