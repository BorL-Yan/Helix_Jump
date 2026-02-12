using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Ball.Controller
{
    public class FinishPlatform : MonoBehaviour
    {
        private LevelAction _levelAction;
        
        [Inject]
        public void Construct(LevelAction levelAction)
        {
            _levelAction = levelAction;
        }

        private void DiactivateFinishPlatform()
        {
            gameObject.SetActive(false);
            Debug.Log("Finish Platform");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        
        private void Start()
        {
           _levelAction.OnFinishLevel += DiactivateFinishPlatform;
        }

        private void OnDestroy()
        {
            _levelAction.OnFinishLevel -= DiactivateFinishPlatform;
        }
    }
}