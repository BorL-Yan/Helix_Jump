using System;
using System.Collections.Generic;
using System.Linq;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    public class FinalPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _pointText;

        [SerializeField] private TMP_Text _currentPointText;

        [SerializeField] private GameObject _coinObj;
        private int instantiateCount = 10;
        [SerializeField] private RectTransform _spawnRadius;
        [SerializeField] private RectTransform _startPosition;
        [SerializeField] private RectTransform _endPosition;

        private const int LEVELPOINT = 50;
        

        private void Start()
        {
            _panel.SetActive(false);
        }

        [ProButton]
        public void TestActivate()
        {
            Activate(null);
        }
        
        
        public void Activate(Action callback)
        {
            _panel.SetActive(true);
            var save = GameSave.GetSettings();

            _levelText.text = $"Level {1} passed";
            _currentPointText.text = NumberFormatter.FormatValue(save.Coin);
            _pointText.text = save.Coin.ToString();

            int currentPoint = save.Coin;

            int point = currentPoint + LEVELPOINT;
            DOTween.To(() => currentPoint, x => currentPoint = x, point, 0.5f)
                .OnUpdate(() =>
                {
                    _pointText.text = currentPoint.ToString();
                })
                .SetEase(Ease.Linear);
            
            Sequence coinSequence = DOTween.Sequence();
            coinSequence.AppendInterval(0.1f);

            List<GameObject> coins = new();
            
            for (int i = 0; i < instantiateCount; i++)
            {
                var coin = Instantiate(_coinObj, transform);

                float scale = Random.Range(1f, 1.5f);
                coin.transform.localScale = Vector3.zero;
                
                coin.transform.localPosition = _startPosition.localPosition 
                                               + new Vector3(Random.Range(-_spawnRadius.localPosition.x, _spawnRadius.localPosition.x),
                                                   Random.Range(-_spawnRadius.localPosition.y, _spawnRadius.localPosition.y));

                float duration = Random.Range(0.2f, 0.4f);
                coinSequence.Join(coin.transform.DOScale(scale, duration));
                    
                coins.Add(coin);
            }
            
            coinSequence.AppendInterval(0.6f);
            
            
            foreach (var coin in coins)
            {
                float duration = Random.Range(0.3f, 0.6f);
                coinSequence
                    .Join(coin.transform.DOLocalMove(_endPosition.localPosition, duration));
            }
            
            coinSequence
                .OnComplete(() =>
                {
                    _currentPointText.text = NumberFormatter.FormatValue(currentPoint);
                    callback?.Invoke();
                });
            
            save.Coin =  currentPoint;
            GameSave.SetSettings(save);
            GameSave.Save();
        }

    }
}