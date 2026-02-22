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
    
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent<LoosPanel>(loosPanel);
        builder.RegisterComponent<LevelProgress>(LevelProgress);
        builder.RegisterComponent<LeaderBoard>(LeaderBoard);
        builder.RegisterComponent<BestPrize>(BestPrize);
        builder.RegisterComponent<FinalPanel>(FinalPanel);
        
        builder.Register<LevelAction>(Lifetime.Singleton);

        builder.RegisterEntryPoint<LevelController>(Lifetime.Singleton)
            .As<IStartable, IDisposable>();
    }
}