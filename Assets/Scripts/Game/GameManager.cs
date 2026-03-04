using System;
using Level.Controller;
using Lib;
using Platform.Main_Scene;
using UnityEngine;
using VContainer;

public class GameManager : SingletonGame<GameManager>
{
    [SerializeField] private MainCameraController mainCameraController;
    [SerializeField] private GameObject _menuCanvas;
    [SerializeField] private LevelLoadingUI _loadingUI;
    private GameObject _ball;

    public bool Inited { get; private set; }
    
    public GameAction Action { get; private set; }
    
    
    public int CurrentActiveLevel { get; set; }
    public bool OpenNewLevel { get; set; }
    public bool IsMenuActive { get; set; }

    [Inject]
    public void Construct(GameAction action)
    {
        Action = action;
    }
    
    protected override void Init() { }

    public void Initializ()
    {
        if(Inited) return;
        Inited = true;
        
        GameSave.Init();
        GameObject prefab = Resources.Load<GameObject>("Ball/Ball_Menu");
    
        if(prefab != null)
        {
            // 1. Instantiate without a parent first
            _ball = Instantiate(prefab);
        
            // 2. Try to set the parent manually
            if (transform != null)
            {
                _ball.transform.SetParent(transform, false);
            }
        }
    }

    public void ActivateLevelScene(Action callback)
    {
        _loadingUI.ActivateLevel(callback);
    }

    public void ActivateMenuScene()
    {
        _loadingUI.ActivateMenu();
    }

    public void ActiveLevel()
    {
        mainCameraController.SetActive(false);
        _menuCanvas.SetActive(false);
        _ball.SetActive(false);
    }

    public void ActiveMenu()
    {
        mainCameraController.SetActive(true);
        _menuCanvas.SetActive(true);
        _ball.SetActive(true);
    }
    
}
