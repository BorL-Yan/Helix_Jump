using System;
using System.Collections.Generic;
using System.Linq;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using TMPro;
using UI_Scripts.Leaderboard;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Level
{
    public class LeaderBoard : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Text _totalScore;

        [SerializeField] private UIButton _closeButton;

        [SerializeField] private List<LeaderboardID> _boardID;
        [SerializeField] private LeaderboardID _playerID;
        
        [SerializeField] private GameObject _leaderboardPanel;
        [SerializeField] private GameObject _emptyPanel;

        [SerializeField] private RectTransform _up;
        [SerializeField] private RectTransform _end;
        [SerializeField] private RectTransform _down;


        [SerializeField] private RectTransform _leftRightIcon;
         
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

        [ProButton]
        public void SetScore(int score)
        {
            _panel.SetActive(true);
            _closeButton.gameObject.SetActive(false);
            int currentScore = GameSave.GetSettings().Score;

            _totalScore.text = NumberFormatter.FormatValue(currentScore);
            
            int point = score +  currentScore;

            Sequence sequence = DOTween.Sequence();

            _playerID.score.text = NumberFormatter.FormatValue(currentScore);
            _playerID.id.text = GameSave.GetSettings().LeaderID.ToString();
            UpdateBoardTexts(GameSave.GetSettings().LeaderID, currentScore, 3);
            sequence.AppendInterval(1f)
                .AppendCallback(() =>
                {
                    ActivateBoardAnimation(point);
                    //TODO Effect
                });
            sequence.Append(DOTween.To(() => currentScore, x => currentScore = x, point, 1f)
                .OnUpdate(() =>
                {
                    string scoreS = NumberFormatter.FormatValue(currentScore);
                    _totalScore.text = scoreS;
                    _playerID.score.text = scoreS;
                })
                .SetEase(Ease.OutQuad));
            GameSave.GetSettings().Score = point;
            GameSave.Save();
        }

        private void ActivateBoardAnimation(int endScore)
        {
            // 1. Инициализация данных
            var settings = GameSave.GetSettings();
            int startID = settings.LeaderID;
            int targetID = (int)MathF.Max( (startID - Random.Range(80, 140)), 1);
            int playerIDPos = 3;

            
            // Создаем одну главную последовательность для синхронизации
            Sequence masterSeq = DOTween.Sequence();

            // 2. Анимация иконки (параллельно основной логике)
            _leftRightIcon.gameObject.SetActive(true);
            Vector2 initialSize = _leftRightIcon.sizeDelta;
            
            Sequence iconSeq = DOTween.Sequence();
            iconSeq.Append(_leftRightIcon.DOSizeDelta(initialSize + new Vector2(100, 0), 0.3f))
                .Append(_leftRightIcon.DOSizeDelta(initialSize, 0.2f))
                .SetLoops(3, LoopType.Restart)
                .OnComplete(() =>
                   {
                       _leftRightIcon.DOSizeDelta(initialSize - new Vector2(200, 100), 0.05f).OnComplete(() =>
                       {
                           _leftRightIcon.gameObject.SetActive(false);
                       });
                   });

            // 3. Анимация игрока (Масштаб + Смена ID)
            int tempID = startID;
            masterSeq.Append(_playerID.transform.DOScale(1.1f, 0.3f));
            
            // Анимируем изменение ID игрока и ОСТАЛЬНЫХ строк таблицы одновременно
            masterSeq.Append(DOTween.To(() => tempID, x => tempID = x, targetID, 1.5f)
                .OnUpdate(() =>
                {
                    _playerID.id.text = tempID.ToString();
                }));

            Sequence boardSeq = DOTween.Sequence();
            _emptyPanel.SetActive(false);
            boardSeq.Append(_leaderboardPanel.transform.DOMoveY(_down.position.y, 0.2f).SetEase(Ease.Linear))
                // Симуляция "тряски" или прокрутки
                .AppendCallback(() => _leaderboardPanel.transform.position = _up.position)
                .Append(_leaderboardPanel.transform.DOMoveY(_down.position.y, 0.5f).SetEase(Ease.Linear))
                .AppendCallback(() =>
                {
                    UpdateBoardTexts(targetID, endScore, playerIDPos);
                    _leaderboardPanel.transform.position = _up.position;
                })
                .Append(_leaderboardPanel.transform.DOMoveY(_down.position.y, 0.5f).SetEase(Ease.Linear))
                .AppendCallback(() => _leaderboardPanel.transform.position = _up.position)
                .AppendCallback(() =>
                {
                    // Финальное обновление значений на итоговые
                    _emptyPanel.SetActive(true);
                })
                .Append(_leaderboardPanel.transform.DOMoveY(_end.position.y, 0.5f))
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    _closeButton.gameObject.SetActive(true);
                });
            settings.LeaderID = targetID;
            GameSave.SetSettings(settings);
            GameSave.Save();
        }

        // Выносим обновление текстов в отдельный метод, чтобы не дублировать код
        private void UpdateBoardTexts(int centerID, int baseScore, int playerPos)
        {
            for (int i = 0; i < _boardID.Count; i++)
            {
                int offset = playerPos - i;
                if(offset <= 0) offset--;
                _boardID[i].id.text = (centerID - offset).ToString();
                _boardID[i].score.text = NumberFormatter.FormatValue(baseScore + offset * 2);
                
            }
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
        
        
        [ProButton]
        public void LeadorBoardSeter()
        {
            _boardID = GetComponentsInChildren<LeaderboardID>().ToList();
        }
    }
}