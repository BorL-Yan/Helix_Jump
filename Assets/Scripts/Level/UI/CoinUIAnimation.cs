using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    public class CoinUIAnimation : MonoBehaviour
    {
        private const int InstantiateCount = 30;
        [SerializeField] private GameObject _coinObj;
        [SerializeField] private RectTransform _spawnRadius;
        [SerializeField] private RectTransform _startPosition;
        [SerializeField] private RectTransform _endPosition;

        [MinMaxRange(0f, 2f), SerializeField] private Vector2 _coinScale;
        [SerializeField] private float _endScale;

        [SerializeField] private float _stopViewDelay = 0.3f;
        [SerializeField] private float _staggerDelay = 0.03f;
        
        public void ActivateAnimation(Action callback)
        {
            Sequence coinSequence = DOTween.Sequence();
            List<GameObject> coins = new();
            
            for (int i = 0; i < InstantiateCount; i++)
            {
                var coin = Instantiate(_coinObj, transform);

                float scale = Random.Range(_coinScale.x, _coinScale.y);
                coin.transform.localScale = Vector3.zero;
                
                Vector3 endPos = _startPosition.localPosition 
                                 + new Vector3(Random.Range(-_spawnRadius.localPosition.x, _spawnRadius.localPosition.x),
                                     Random.Range(-_spawnRadius.localPosition.y, _spawnRadius.localPosition.y));
                coin.transform.localPosition = Vector3.Lerp(_startPosition.localPosition, endPos, 0.5f);
                
                float duration = Random.Range(0.2f, 0.4f);
                
                coin.transform.DOLocalMove(endPos, duration);
                
                coinSequence.Join(coin.transform.DOScale(scale, duration));
                    
                coins.Add(coin);
            }
            
            coinSequence.AppendInterval(_stopViewDelay);

            float startDuration = coinSequence.Duration();
            for(int i = 0; i< coins.Count; i++)
            {
                var coin = coins[i];
                float dealy = i * _staggerDelay;
                float duration = Random.Range(0.3f, 0.6f);
                coinSequence.Insert(startDuration + dealy, coin.transform.DOLocalMove(_endPosition.localPosition, duration))
                        .OnComplete(() =>
                        {
                            coin.transform.localScale = Vector3.zero;
                        })
                    .Insert(startDuration + dealy,coin.transform.DOScale(_endScale, 0.2f).SetEase(Ease.OutBack));
            }
            coinSequence
                .OnComplete(() =>
                {
                    callback?.Invoke();
                    coins.ForEach(item =>
                    {
                        Destroy(item);
                    });
                });
        }
        
    }
}