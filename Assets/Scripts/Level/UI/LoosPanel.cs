using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    public class LoosPanel : MonoBehaviour
    {
        [SerializeField] private GameObject loosPanel;

        [SerializeField] private Transform _health;
        [SerializeField] private Image _healthBar;
        [SerializeField] private float _endDuration;
        [SerializeField] private GameObject _reviveObjIcon;

        private Sequence _sequence;
        private Sequence _healthBarSequence;
        
        private void Start()
        {
            loosPanel.SetActive(false);
        }
        
        [ProButton]
        public void SetActive(bool value)
        {
            loosPanel.SetActive(value);
            ActivateHealth();
        }


        private void ActivateHealth()
        {
            _sequence.Kill();
            _sequence = DOTween.Sequence(); 
            _sequence.Append(_health.DOScale(1.2f, 0.6f).SetEase(Ease.InSine)) 
                .Append(_health.DOScale(1f, 0.6f).SetEase(Ease.OutSine))    
                .SetLoops(-1, LoopType.Restart);

            float time = 1f;

            _reviveObjIcon.SetActive(true);
            
            _healthBarSequence.Kill();
            _healthBarSequence = DOTween.Sequence();
            _healthBarSequence.Append(DOTween.To(() => time, x => time = x, 0f, _endDuration)
                    .OnUpdate(() =>
                    {
                        _healthBar.fillAmount = time;
                    }).SetEase(Ease.Linear))
                .OnComplete(() =>
                {
                    _reviveObjIcon.SetActive(false);
                    _sequence?.Kill();
                });
        }
    }
}