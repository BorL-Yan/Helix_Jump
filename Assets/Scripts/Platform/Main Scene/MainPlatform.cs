using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Platform.Main_Scene
{
    public class MainPlatform : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material _activeMaterial;
        [SerializeField] private Material _selectMaterial;
        [SerializeField] private Material _deactiveMaterial;

        [SerializeField] private Collider _selectorPlatform;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _rotationDuration;

        [SerializeField] private SpriteRenderer _selectCircle;
        [SerializeField] private float _fadeOutDuration;
        private float _scale;
        private float _alpha;

        [SerializeField] private Animator _animator;


        [SerializeField] private GameObject _particle;
        
        [field:SerializeField] public int platformID { get; set; }
        
        private void Awake()
        {
            _text.text = platformID.ToString();
            _particle.SetActive(false);
            Deactivate();
            
            _scale = _selectCircle.transform.localScale.x;
            _alpha = _selectCircle.color.a;
            _selectCircle.transform.localScale = Vector3.zero;
        }

        public void ActivateAnimation()
        {
            _animator.Play("Open");
            _particle.SetActive(true);
        }

        public void OpenedAnimation()
        {
            _animator.Play("Opened");
            //_animator.Play("Open");
        }
        
        public void ClosedAnimation()
        {
            _animator.Play("Close");
        }
        
        public void Activate()
        {
            OpenedAnimation();
            _meshRenderer.material = _activeMaterial;
            _selectorPlatform.enabled = true;
        }

        public void Deactivate()
        {
            ClosedAnimation();
            _meshRenderer.material = _deactiveMaterial;
            _selectorPlatform.enabled = false;
        }
        

        public void Select()
        {
            _meshRenderer.material = _selectMaterial;
            _selectorPlatform.enabled = false;
            GameManager.Instance.CurrentActiveLevel = platformID;


            _selectCircle.color =
                new Color(_selectCircle.color.r, _selectCircle.color.g, _selectCircle.color.b, _alpha);
            _selectCircle.transform.localScale = Vector3.zero;
            
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_selectCircle.transform.DOScale(_scale, _fadeOutDuration))
                .Join(_selectCircle.DOColor(
                    new Color(_selectCircle.color.r, _selectCircle.color.g, _selectCircle.color.b, 0),
                    _fadeOutDuration).SetEase(Ease.InCubic));
        }

        public void JumpEffect()
        {
            float rotation = 10f;
            
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_text.transform.DOLocalRotate(new Vector3(0, rotation, 0), _rotationDuration))
                .Append(_text.transform.DOLocalRotate(new Vector3(0, -rotation, 0), _rotationDuration * 2))
                .Append(_text.transform.DOLocalRotate(new Vector3(0, 0, 0), _rotationDuration));
        }
    }
}