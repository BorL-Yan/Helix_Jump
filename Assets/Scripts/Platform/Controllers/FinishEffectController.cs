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
        
        private void Activate(Vector3 pos)
        {
            _effect.transform.position = pos + Vector3.down*0.1f;
            _effect.Clear();
            _effect.Play();
        }


        public void OnEnable()
        {
            _effect.Clear();
            _levelAction.OnFinishEffectActivate += Activate;
        }

        private void OnDisable()
        {
            _levelAction.OnFinishEffectActivate -= Activate;
        }
    }
}