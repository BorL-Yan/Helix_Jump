using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Level
{
    public class LeaderBoard : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Text _totalScore;

        [SerializeField] private UIButton _closeButton;
        
        private Action _callback;

        private void Start()
        {
            _panel.SetActive(false);
        }

        public void Activate(Action callback)
        {
            _callback = callback;
            _panel.SetActive(true);
        }

        public void SetScore(int score)
        {
            int currentScore = GameSave.GetSettings().Score;

            _totalScore.text = NumberFormatter.FormatValue(currentScore);
            
            int point = score +  currentScore;
            
            DOTween.To(() => currentScore, x => currentScore = x, point, 1f)
                .OnUpdate(() =>
                {
                    _totalScore.text = NumberFormatter.FormatValue(currentScore);
                })
                .SetEase(Ease.OutQuad);
            GameSave.GetSettings().Score = point;
            GameSave.Save();
        }

        private void Close()
        {
            _callback?.Invoke();
            _panel.SetActive(false);
        }

        private void OnEnable()
        {
            _closeButton.OnClick += Close;
        }

        private void OnDisable()
        {
            _closeButton.OnClick -= Close;
        }
    }
}