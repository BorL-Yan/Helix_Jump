using UnityEngine;

namespace Boot
{
    public class LevelEntryPoint : SingletonScene<LevelEntryPoint>
    {
        [Min(1), SerializeField] private int _levelEntry;
        
        protected override void Init()
        {
            base.Init();
            if (GameManager.Instance == null)
            {
                GameManager manager = Resources.Load<GameManager>("Menegers/GameBootstrap");
                Instantiate(manager).Initializ();
                Intialize();
            }
        }
        
        public void Intialize()
        {
            int activeLevel = _levelEntry;
            
            if (GameManager.Instance.CurrentActiveLevel != 0)
            {
                activeLevel = GameManager.Instance.CurrentActiveLevel;
            }
            
            GameObject level = Resources.Load<GameObject>("Levels/Level_" + activeLevel);
            if(level != null)
                Instantiate(level);
            
            GameObject ball = Resources.Load<GameObject>("Ball/Ball");
            if (ball != null)
            {
                ball = Instantiate(ball);
                ball.transform.position = Vector3.zero;
            }
        }

      
    }
}