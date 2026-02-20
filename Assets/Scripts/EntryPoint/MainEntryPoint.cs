using System;
using UnityEngine;


[DefaultExecutionOrder(-100)]
public class MainEntryPoint : MonoBehaviour
{
    private bool _initilaze;
    
    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameManager manager = Resources.Load<GameManager>("Menegers/GameBootstrap");
            Instantiate(manager).Initializ();
            _initilaze = false;
        }
        else // TODO Testing camera animation
        {
            _initilaze = true;
        }
        Init();
    }
    
    
    private void Init()
    {
        GameManager.Instance.ActiveMenu();
    }

    private void Start()
    {
        Action<int> action; 
        action = !_initilaze ? 
            GameManager.Instance.Action.ActivateGameSelectPlatform : 
            GameManager.Instance.Action.SelectPlatform;
        
        action?.Invoke(GameSave.GetSettings().Level);
        
        _initilaze = true;
    }
    
}
