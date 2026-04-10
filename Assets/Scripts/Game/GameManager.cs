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
    
    public GameAction Action { get; private set; }
    public GameState GameState { get; set; }
    
    public int CurrentActiveLevel { get; set; } = 1;
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
        GameObject prefab = Resources.Load<GameObject>("Ball/Ball_Menu");
        
        if(prefab != null)
        {
            _ball = Instantiate(prefab);
            if (transform != null)
            {
                _ball.transform.SetParent(transform, false);
            }
            GameState = GameState.main;
        }
        GameObject soundMangerPrefab = Resources.Load<GameObject>("Managers/SoundManager");
        if (soundMangerPrefab != null)
        {
            SoundManager soundManager = Instantiate(soundMangerPrefab).GetComponent<SoundManager>();
            soundManager.Initialize();
            soundManager.transform.SetParent(this.transform);
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

public enum GameState
{
    main,
    level,
    skin,
}
