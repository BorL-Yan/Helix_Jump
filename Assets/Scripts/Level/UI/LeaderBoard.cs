using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UI_Scripts.Leaderboard;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Level
{
    public class LeaderBoard : MonoBehaviour
    {
        [SerializeField] private GameObject _mainPanel;
        
        [SerializeField] private GameObject _panel;
        [SerializeField] private TMP_Text _totalScore;

        [SerializeField] private GameObject _bronzePanel;
        [SerializeField] private GameObject _silverPanel;
        
        [SerializeField] private UIButton _closeButton;
        
        
        [SerializeField] private List<LeaderboardID> _boardID;
        [SerializeField] private LeaderboardID _playerID;
        
        [SerializeField] private Transform _leaderboardPanel;
        [SerializeField] private GameObject _emptyPanel;

        [SerializeField] private Transform _up;
        [SerializeField] private Transform _end;
        [SerializeField] private Transform _down;

        [SerializeField] private Transform _finishBoard;
        
        [SerializeField] private Transform _middle;
        [SerializeField] private Transform _finishPlayer;

        [SerializeField] private RectTransform _leftRightIcon;

        [SerializeField] private ParticleSystem _particle;

        [SerializeField] private RankUpController _rankUp;
        [SerializeField] private OpenKeys _openKeys;
        [SerializeField] private ChampionLeagueUI _championLeagueUI;
        private Action _callback;

        private void Start()
        {
            _panel.SetActive(false);
            _rankUp.gameObject.SetActive(false);
            _championLeagueUI.gameObject.SetActive(false);
            _particle.Clear();
        }

        public void Activate(Action callback)
        {
            _callback = callback;
            _panel.SetActive(true);
            ActivateRankedPanel();
        }

        private void ActivateRankedPanel()
        {
            int rankedID = GameSave.GetSettings().RankedID;

            _bronzePanel.SetActive(rankedID == 0);
            _silverPanel.SetActive(rankedID == 1);
        }
        
        [VInspector.Button]
        public void SetScore(int score)
        {
            _panel.SetActive(true);

            _particle.Clear();
            _particle.Play();
            
            _closeButton.gameObject.SetActive(false);
            int currentScore = GameSave.GetSettings().Score;

            _totalScore.text = NumberFormatter.FormatValue(currentScore);
            
            int point = score +  currentScore;

            _playerID.score.text = NumberFormatter.FormatValue(currentScore);
            _playerID.id.text = GameSave.GetSettings().LeaderID.ToString();
            
            UpdateBoardTexts(GameSave.GetSettings().LeaderID, currentScore, 3);
            
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(0.1f)
                .AppendCallback(() =>
                {
                    ActivateBoardAnimation(point);
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
            var settings = GameSave.GetSettings();
            int startID = settings.LeaderID;
            int targetID = startID - (int)(Random.Range(8000, 13000) / (settings.RankedID * 20 + 1));

            float endPosBoard = _end.position.y;
            float endPosPlayer = _middle.position.y;
            _leaderboardPanel.transform.position = _end.position;
            _playerID.transform.position = _middle.position;
            
            
            if (targetID <= 1)
            {
                targetID = 1;
                endPosBoard = _finishBoard.position.y;
                endPosPlayer = _finishPlayer.position.y;
            }
            int playerIDPos = 4;

         
            Sequence masterSeq = DOTween.Sequence();

            _leftRightIcon.gameObject.SetActive(true);
            Vector2 initialSize = _leftRightIcon.sizeDelta;
            
            Sequence iconSeq = DOTween.Sequence();
            iconSeq.Append(_leftRightIcon.DOSizeDelta(initialSize + new Vector2(100, 0), 0.3f))
                .Append(_leftRightIcon.DOSizeDelta(initialSize, 0.4f))
                .SetLoops(3, LoopType.Restart)
                .OnComplete(() =>
                   {
                       _leftRightIcon.DOSizeDelta(initialSize - new Vector2(200, 100), 0.1f).OnComplete(() =>
                       {
                           _leftRightIcon.gameObject.SetActive(false);
                       });
                   });

            int tempID = startID;
            //masterSeq.Append(_playerID.transform.DOScale(1.2f, 0.3f));
            _playerID.transform.localScale = Vector3.one * 1.1f;
            masterSeq.Append(DOTween.To(() => tempID, x => tempID = x, targetID, 1.5f)
                    .OnUpdate(() =>
                    {
                        _playerID.id.text = "#" + tempID.ToString();
                    }))
                .Join(_playerID.transform.DOMoveY(endPosPlayer, 1.5f))
                .Join(_leftRightIcon.transform.DOMoveY(endPosPlayer, 1.5f));

            Sequence boardSeq = DOTween.Sequence();
            _emptyPanel.SetActive(false);
            boardSeq.Append(_leaderboardPanel.transform.DOMoveY(_down.position.y, 0.2f).SetEase(Ease.Linear))
                .AppendCallback(() => _leaderboardPanel.transform.position = _up.position)
                .Append(_leaderboardPanel.transform.DOMoveY(_down.position.y, 0.5f).SetEase(Ease.Linear))
                .AppendCallback(() =>
                {
                    UpdateBoardTexts(targetID, endScore, playerIDPos);
                    _leaderboardPanel.transform.position = _up.position;
                })
                .Append(_leaderboardPanel.transform.DOMoveY(_down.position.y, 0.5f).SetEase(Ease.Linear))
                .AppendCallback(() => _leaderboardPanel.transform.position = _up.position)
                .Append(_leaderboardPanel.transform.DOMoveY(_down.position.y, 0.5f).SetEase(Ease.Linear))
                .AppendCallback(() => _leaderboardPanel.transform.position = _up.position)
                .AppendCallback(() =>
                {
                    _particle.Clear();
                    _particle.Play();
                    _particle.transform.position = _playerID.transform.position;
                    _emptyPanel.SetActive(true);
                })
                .Append(_leaderboardPanel.transform.DOMoveY(endPosBoard, 0.5f))
                //.Append(_playerID.transform.DOScale(1f, 0.3f))
                .AppendInterval(1f)
                .AppendCallback(() =>
                {
                    _closeButton.gameObject.SetActive(true);
                    if (targetID == 1)
                    {
                        settings.RankedID++;

                        settings.LeaderID = Random.Range(4500, 3500);
                        
                        ActivateRankedUpgrade();
                    }
                    else
                    {
                        settings.LeaderID = targetID;
                    }
                });
            
            GameSave.SetSettings(settings);
            GameSave.Save();
        }

        private void UpdateBoardTexts(int centerID, int baseScore, int playerPos)
        {
            for (int i = 0; i < _boardID.Count; i++)
            {
                int offset = playerPos - i;
                if(offset <= 0) offset--;
                _boardID[i].id.text = "#"+(centerID - offset).ToString();
                _boardID[i].score.text = NumberFormatter.FormatValue(baseScore + offset * 2);
            }
        }
        
        [VInspector.Button]
        public void ActivateRankedUpgrade()
        {
            OpenKeys keys = Instantiate(_openKeys).GetComponent<OpenKeys>();
            keys.transform.position = Vector3.zero;
            _mainPanel.SetActive(false);
            keys.Activate(() =>
            {
                _mainPanel.SetActive(true);
                keys.gameObject.SetActive(false);
                _rankUp.Activate(() =>
                {
                    _rankUp.gameObject.SetActive(false);
                    SoundManager.Instance.Play(SoundType.RankedUp_Particle);
                    _championLeagueUI.Activate(()=>
                    {
                        _championLeagueUI.gameObject.SetActive(false);
                        Close();
                    }); 
                });
            });
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
        
        
        [VInspector.Button]
        public void LeadorBoardSeter()
        {
            _boardID = GetComponentsInChildren<LeaderboardID>().ToList();
        }
    }
}