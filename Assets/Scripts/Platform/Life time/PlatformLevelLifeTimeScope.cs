using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;


public class PlatformLevelLifeTimeScope : LifetimeScope
{
    [SerializeField] private Transform _root; 
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_root);
        
        builder.RegisterEntryPoint<PlatformEntry>()
            .As<IStartable>();
        
        builder.Register<PlatformRotation>(Lifetime.Scoped)
            .AsSelf()
            .As<IStartable, IDisposable>();

    }
}
