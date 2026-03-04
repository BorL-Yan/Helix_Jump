using System;
using Boot;
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

        private void Start()
        {
            _loadingPanel.SetActive(false);
        }

        private void SetActive(bool active)
        {
            _loadingPanel.SetActive(active);
        }

        public void ActivateLevel(Action callback)
        {
            Activate(() =>
            {
                callback?.Invoke();
                SceneManager.LoadScene(1);
            });
        }

        public void ActivateMenu()
        {
            Activate(() =>
            {
                SceneManager.LoadScene(0);
            });
        }

        public void ActivateLevel(string sceneName)
        {
            Activate(() =>
            {
                SceneManager.LoadScene(sceneName);
            });
        }

        private void Activate(Action callback)
        {
            Sequence activateSequence = DOTween.Sequence();

            _loadingPanel.transform.localPosition = _start.localPosition;
            _loadingPanel.SetActive(true);

            activateSequence.Append(_loadingPanel.transform.DOLocalMove(_active.localPosition, 0.2f))
                .AppendCallback(() =>
                {
                    callback?.Invoke();
                })
                .AppendInterval(0.2f)
                .Append(_loadingPanel.transform.DOLocalMove(_end.localPosition, 0.2f))
                .OnComplete(() => SetActive(false));
        }

        [ProButton]
        private void TestActivate()
        {
            Activate(() =>
            {
                Debug.Log("Activate");
            });
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