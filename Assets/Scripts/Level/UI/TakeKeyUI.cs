using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Level
{
    public class TakeKeyUI : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private List<GameObject> _key;

        [SerializeField] private RectTransform _deactive;
        [SerializeField] private RectTransform _active;
        
        
        private LevelAction _levelAction;

        [Inject]
        public void Construct(LevelAction levelAction)
        {
            _levelAction = levelAction;
        }


        private void Start()
        {
            _panel.SetActive(false);
            //TakeKey();
        }

        private void TakeKey()
        {
            _panel.SetActive(true);
            int keyCount = GameSave.GetSettings().Key + 1;

            foreach (var item in _key)
            {
                item.SetActive(false);
            }

            Sequence sequence = DOTween.Sequence();
            _panel.transform.localPosition = _deactive.localPosition;
            sequence.Append(_panel.transform.DOLocalMove(_active.localPosition, 0.125f));
            sequence.AppendInterval(0.2f);
            for (int i = 0; i < keyCount; i++)
            {
                var key = _key[i];
                key.transform.localScale = Vector3.zero;
                _key[i].SetActive(true);
                sequence.Join(key.transform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutElastic))
                    .AppendInterval(0.2f);
            }
           
            
            
            sequence.AppendInterval(0.5f)
                .Append(_panel.transform.DOLocalMove(_deactive.localPosition, 0.125f))
                .OnComplete(() =>
                {
                    _panel.SetActive(false);
                });
            var settings = GameSave.GetSettings();
            settings.Key = keyCount;
            settings.LevelData[GameManager.Instance.CurrentActiveLevel-1].TakeKey = true;
            GameSave.SetSettings(settings);
            GameSave.Save();
        }

        private void OnEnable()
        {
            _levelAction.OnTakeKey += TakeKey;
        }

        private void OnDisable()
        {
            _levelAction.OnTakeKey -= TakeKey;
        }
    }
}