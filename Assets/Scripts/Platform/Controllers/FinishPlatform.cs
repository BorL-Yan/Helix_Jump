using Platform.Multy;
using UnityEngine;
using VContainer;

namespace Ball.Controller
{
    public class FinishPlatform : MonoBehaviour
    {
        private LevelAction _levelAction;
        MultyPlatformController _multyPlatformActivate;
        
        [Inject]
        public void Construct(LevelAction levelAction)
        {
            _levelAction = levelAction;
            _levelAction.OnFinishLevel += DiactivateFinishPlatform;
            _levelAction.GetFinshPosition += GetPos;
        }

        private void Start()
        {
            MultyPlatformController prefab = Resources.Load<MultyPlatformController>("Platforms/Multyply");

            _multyPlatformActivate = Instantiate(prefab, transform.position, transform.rotation, transform.parent);
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
            _levelAction.OnFinishLevel -= DiactivateFinishPlatform;
            _levelAction.GetFinshPosition -= GetPos;
        }
        
    }
}