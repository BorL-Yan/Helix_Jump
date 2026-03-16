using System;
using Level;
using Level.Controller;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LevelLifeTimeScope : LifetimeScope
{
    [SerializeField] private LoosPanel loosPanel;
    [SerializeField] private LevelProgress LevelProgress;
    [SerializeField] private LeaderBoard LeaderBoard;
    [SerializeField] private BestPrize BestPrize;
    [SerializeField] private FinalPanel FinalPanel;
    [SerializeField] private TakeKeyUI _keyUI;
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent(loosPanel);
        builder.RegisterComponent(LevelProgress);
        builder.RegisterComponent(LeaderBoard);
        builder.RegisterComponent(BestPrize);
        builder.RegisterComponent(FinalPanel);
        builder.RegisterComponent(_keyUI);
        
        builder.Register<LevelAction>(Lifetime.Singleton);

        builder.RegisterEntryPoint<LevelController>(Lifetime.Singleton)
            .As<IStartable, IDisposable>();
    }
}