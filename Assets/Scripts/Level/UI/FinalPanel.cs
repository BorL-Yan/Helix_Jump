using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level
{
    public class FinalPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        
        [SerializeField] private TMP_Text _levelText;
        [FormerlySerializedAs("_CoinText")] [FormerlySerializedAs("_pointText")] [SerializeField] private TMP_Text _coinText;

        [FormerlySerializedAs("_currentPointText")] [SerializeField] private TMP_Text _currentCoinText;
        
        private const int LEVELPOINT = 50;

        [SerializeField] private CoinUIAnimation _coinUIAnimation;

        private void Start()
        {
            _panel.SetActive(false);
        }

        [VInspector.Button]
        public void TestActivate()
        {
            Activate(null);
        }
        
        
        public void Activate(Action callback)
        {
            _panel.SetActive(true);
            var save = GameSave.GetSettings();

            _levelText.text = $"Level {GameManager.Instance.CurrentActiveLevel} passed";
            _currentCoinText.text = NumberFormatter.FormatValue(save.Coin);
            _coinText.text = save.Coin.ToString();

            int currentPoint = save.Coin;

            int point = currentPoint + LEVELPOINT;
            DOTween.To(() => currentPoint, x => currentPoint = x, point, 0.5f)
                .OnUpdate(() =>
                {
                    _coinText.text = currentPoint.ToString();
                })
                .SetEase(Ease.Linear);
            
            Sequence coinSequence = DOTween.Sequence();
            coinSequence.AppendInterval(0.1f)
                .AppendCallback(() =>
                {
                    _coinUIAnimation.ActivateAnimation(() =>
                    {
                        _currentCoinText.text = NumberFormatter.FormatValue(currentPoint);
                        callback?.Invoke();
                    });
                });
            
            save.Coin = point;
            GameSave.SetSettings(save);
            GameSave.Save();
        }

    }
}