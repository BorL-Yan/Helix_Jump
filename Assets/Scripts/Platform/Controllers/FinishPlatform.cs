using Platform.Multy;
using UnityEngine;
using VContainer;

namespace Ball.Controller
{
    public class FinishPlatform : MonoBehaviour
    {
        private LevelAction _levelAction;
        MultyPlatformController _multyPlatformActivate;
        
        public void Init(LevelAction levelAction)
        {
            _levelAction = levelAction;
            _levelAction.OnFinishLevel += DiactivateFinishPlatform;
            _levelAction.GetFinshPosition += GetPos;
            StartInit();
        }

        private void StartInit()
        {
            MultyPlatformController prefab = Resources.Load<MultyPlatformController>("Platforms/Multyply");
            
            _multyPlatformActivate = Instantiate(prefab);
            _multyPlatformActivate.transform.SetParent(transform.parent);
            _multyPlatformActivate.transform.position = transform.position;
            _multyPlatformActivate.Deactivate();
        }
        
        private void DiactivateFinishPlatform()
        {
            gameObject.SetActive(false);
            _multyPlatformActivate.Activate();
        }
        
        private Vector3 GetPos() => transform.position; 
        
        private void OnDestroy()
        {
            if (_levelAction != null)
            {
                _levelAction.OnFinishLevel -= DiactivateFinishPlatform;
                _levelAction.GetFinshPosition -= GetPos;
            }
        }
        
    }
}