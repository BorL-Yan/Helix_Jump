using System;
using UnityEngine;
using VContainer;

namespace Ball.Controller
{
    public class ChangeBallColor : MonoBehaviour
    {
        [SerializeField] private BallMaterialController _materialController;
        [SerializeField] private TrailRenderer _trailRenderer;
        public  SpriteRenderer _spriteRenderer;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private ParticleSystem _jumpParticle;

        private BallConfig _config;
        private ParticleSystemRenderer _particleRenderer;
        [Inject]
        public void Construct(BallConfig config)
        {
            _config = config;
        }

        private void Awake()
        {
            if(_particleRenderer!= null)
                _particleRenderer = _particleSystem.GetComponent<ParticleSystemRenderer>();
        }

        public void OnEnable()
        {
            BallColor ballColor = _config.GetMaterial();
            
            _materialController?.SetMaterial(ballColor.BallMaterial);

            if (_trailRenderer != null)
            {
                _trailRenderer.colorGradient = ballColor.Gradient;
                _trailRenderer.material = ballColor.ParticleMaterial;
            }
            if (_spriteRenderer != null) _spriteRenderer.color = ballColor.Color;

            if (_particleRenderer != null) _particleRenderer.material = ballColor.ParticleMaterial;
            if (_jumpParticle != null)
            {
                var main = _jumpParticle.main;
                main.startColor = new ParticleSystem.MinMaxGradient(ballColor.Color);
            }

        }
    }
}