using System;
using Platform.Configuration;
using UnityEngine;
using VContainer;
using VContainer.Unity;


public class PlatformLevelLifeTimeScope : LifetimeScope
{
    [SerializeField] private Transform _root;
    [SerializeField] private LevelColorCollection _colorCollection;
    
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(_root);
        builder.RegisterComponent(_colorCollection);
        
        builder.RegisterEntryPoint<PlatformEntry>()
            .As<IStartable>();
        
        builder.Register<PlatformRotation>(Lifetime.Scoped)
            .AsSelf()
            .As<IStartable, IDisposable>();
    }
}
