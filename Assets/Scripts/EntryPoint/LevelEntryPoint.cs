using System;
using Level.Manager;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Boot
{
    public class LevelEntryPoint : MonoBehaviour
    {
        [Min(1), SerializeField] private int _levelEntry;
        
        private LevelAction _levelAction;

        [Inject]
        public void Construct(LevelAction levelAction)
        {
            _levelAction = levelAction;
        }
        
        private void Awake()
        {
            if (GameManager.Instance == null)
            {
                GameSave.Init();
                GameObject obj = Resources.Load<GameObject>("Managers/GameBootstrap");
                Instantiate(obj);
                GameManager.Instance.CurrentActiveLevel = _levelEntry; 
                GameManager.Instance.Initializ();
            }
            
            ActivateLevel(GameManager.Instance.CurrentActiveLevel);
            GameManager.Instance.ActiveLevel();
        }
        
        public void ActivateLevel(int activeLevel)
        {
            GameObject level = Resources.Load<GameObject>("Levels/Level");
            if (level != null)
            {
                level = Instantiate(level);
                level.GetComponent<LevelPlatformManager>().Initialize(activeLevel);
            }
            
            GameObject ball = Resources.Load<GameObject>("Ball/Ball");
            if (ball != null)
            {
                ball = Instantiate(ball);
                ball.transform.position = Vector3.zero;
            }

            //Instantiate(Resources.Load<GameObject>("UI/LevelCanvas"));
            
            _levelAction.OnStartLevel?.Invoke();

            LevelManager.Instance.LevelProgress.Activate(activeLevel,
                level.GetComponent<LevelPlatformManager>().PlatformsCount);
        }
    }
}