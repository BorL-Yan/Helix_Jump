using System;
using DG.Tweening;
using UnityEngine;

namespace Level
{
    public class NewSkinePanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private GameObject _skinPanel;

        [SerializeField] private Transform _start;
        [SerializeField] private Transform _end;
        
        private void Start()
        {
            _panel.SetActive(false);
            GameManager.Instance.Action.OnActivateNewSkin += ActivateNewSkin;
        
        }
        
        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Action.OnActivateNewSkin -= ActivateNewSkin;
            }
        }
        
        [VInspector.Button]
        private void ActivateNewSkin(BallSkinType skinType)
        {
            _skinPanel.SetActive(true);
            _panel.SetActive(true);
            _skinPanel.transform.position = _start.position;

            GameSave.GetSettings().newSkin = true;
            GameSave.Save();

            Sequence sequence = DOTween.Sequence();

            float duration = 0.4f; 
            
            sequence.Append(_skinPanel.transform.DOMove(_end.position, duration))
                .AppendInterval(2f)
                .Append(_skinPanel.transform.DOMove(_start.position, duration))
                .OnComplete(() =>
                {
                    _skinPanel.SetActive(false);
                    _panel.SetActive(false);
                });
        }
    }
}