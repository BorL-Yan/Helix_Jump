using Lib;
using UnityEngine;
using VContainer;


public class GameManager : SingletonScene<GameManager>
{
    public bool Inited { get; private set; }
    
    public GameAction Action { get; private set; }
    
    
    public int CurrentActiveLevel { get; private set; }

    [Inject]
    public void Construct(GameAction action)
    {
        Action = action;
        Debug.Log("Action: Inited" );
    }
    
    protected override void Init() { }

    public void Initializ()
    {
        if(Inited) return;
        Inited = true;
        
        GameSave.Init();
        
    }
}
