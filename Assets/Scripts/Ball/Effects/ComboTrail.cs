using UnityEngine;
using VContainer;

namespace Ball.Controller
{
    public class ComboTrail : SingletonScene<ComboTrail>
    {
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private Material _defoultMateril;
        private Material _comboMateril;

        [SerializeField] private ParticleSystem _ballFire;
        private Transform _target;
        
        private bool activated;
        private bool comboActivated;

        private ParticleSystemRenderer _renderer;

        [Inject]
        public void Construct(Transform target)
        {
            _target = target;
        }
        
        
        protected override void Init()
        {
            base.Init();
            _renderer = _particles.GetComponent<ParticleSystemRenderer>();
            _particles.Pause();
            gameObject.SetActive(false);
            _ballFire.Pause();
        }

        private void Update()
        {
            if (activated)
            {
                transform.position = _target.position;
            }
        }


        public void SetMaterial(Material material)
        {
            _comboMateril = material;
        }
        
        
        public void Activate()
        {
            if (!activated)
            {
                gameObject.SetActive(true);
                activated = true;
                _renderer.material = _comboMateril;
                _particles.Play();
            }else if (activated && !comboActivated)
            {
                comboActivated = true;
                _renderer.material = _defoultMateril;
                _ballFire.Play();
                //_fireSprite.gameObject.SetActive(true);
            }
        }

        public void Deactivate()
        {
            activated = false;
            comboActivated = false;
            _particles.Stop();
            gameObject.SetActive(false);
            //_fireSprite.gameObject.SetActive(false);
            _ballFire.Pause();
        }
        
        
    }
}