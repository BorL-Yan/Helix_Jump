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
                GameObject obj = Resources.Load<GameObject>("Menegers/GameBootstrap");
                GameManager manager = Instantiate(obj).GetComponent<GameManager>();
                manager.Initializ();
                manager.CurrentActiveLevel = _levelEntry;
                ActivateLevel(_levelEntry);
            }
            else
            {
                ActivateLevel(GameManager.Instance.CurrentActiveLevel);
                Debug.Log("Active Scene");
            }
            GameManager.Instance.ActiveLevel();
        }
        
        public void ActivateLevel(int activeLevel)
        {
            GameObject level = Resources.Load<GameObject>("Levels/Level_" + activeLevel);
            if(level != null)
                Instantiate(level);
            
            GameObject ball = Resources.Load<GameObject>("Ball/Ball");
            if (ball != null)
            {
                ball = Instantiate(ball);
                ball.transform.position = Vector3.zero;
            }
            
            _levelAction.OnStartLevel?.Invoke();

            LevelManager.Instance.LevelProgress.Activate(activeLevel,
                level.GetComponent<LevelPlatformManager>().PlatformsCount);
        }
    }
}