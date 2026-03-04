using UnityEngine;
using VContainer;

namespace Platform.Controllers
{
    public class FinishEffectController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _effect;

        private LevelAction _levelAction;

        [Inject]
        public void Construct(LevelAction levelAction)
        {
            _levelAction = levelAction;
        }
        
        public void Start()
        {
            _effect.Clear();
        }


        private void Activate(Vector3 pos)
        {
            _effect.transform.position = pos + Vector3.down*0.1f;
            _effect.Clear();
            _effect.Play();
        }


        private void OnEnable()
        {
            Debug.Log("OnEnable");
            _levelAction.OnFinishEffectActivate += Activate;
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable");
            _levelAction.OnFinishEffectActivate -= Activate;
        }
    }
}