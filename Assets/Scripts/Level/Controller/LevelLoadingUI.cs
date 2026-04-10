using System;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level.Controller
{
    public class LevelLoadingUI : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingPanel;
        [SerializeField] private RectTransform _start;
        [SerializeField] private RectTransform _active;
        [SerializeField] private RectTransform _end;

        private void Awake()
        {
            SetActive(false);
        }

        private void SetActive(bool active)
        {
            _loadingPanel.SetActive(active);
        }

        public void ActivateLevel(Action callback)
        {
            SetActive(true);
            Activate(() =>
            {
                SceneManager.LoadScene(1);
                GameManager.Instance.GameState = GameState.level;
                callback?.Invoke();
            });
        }

        public void ActivateMenu()
        {
            Activate(() =>
            {
                SceneManager.LoadScene(0);
            });
            GameManager.Instance.Action.ActivateGlobalPanel(true);
        }
        
        private void Activate(Action callback)
        {
            SetActive(true);
            SoundManager.Instance.Play(SoundType.Cloud_Whoosh);
            _loadingPanel.transform.localPosition = _start.localPosition;

            Sequence activateSequence = DOTween.Sequence();
            activateSequence.Append(_loadingPanel.transform.DOLocalMove(_active.localPosition, 0.2f))
                .AppendCallback(() =>
                {
                    callback?.Invoke();
                })
                .AppendInterval(0.2f)
                .Append(_loadingPanel.transform.DOLocalMove(_end.localPosition, 0.2f))
                .OnComplete(() => SetActive(false));
        }
        
        public void ActivateLevel(int levelIndex)
        {
            Activate(() =>
            {
                SceneManager.LoadScene(levelIndex);
            });
        }
    }
}