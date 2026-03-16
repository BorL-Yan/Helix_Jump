using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI_Scripts
{
    public class SkinPanelController : UIButton
    {
        [SerializeField] private PanelManager panelManager;
        [Header("Highlight")]
        [SerializeField] private float normalScale = 1f;
        [SerializeField] private float openedPanelScale = 1.1f;
        [SerializeField] private float scaleLerpSpeed = 10f;

        [Header("Panel")]
        [SerializeField] private GameObject skinPanel;

        [SerializeField] private GameObject _newSkin;

        private void Awake()
        {
            ChangeSettings();
            GameSave.OnChangeSettings += ChangeSettings;
        }
        private void OnDestroy()
        {
            GameSave.OnChangeSettings -= ChangeSettings;
        }

        private void Start()
        {
            transform.localScale = Vector3.one * normalScale;
        }

        private void Update()
        {
            if (skinPanel == null)
                return;

            float targetScale = skinPanel.activeSelf ? openedPanelScale : normalScale;
            var target = Vector3.one * targetScale;
            transform.localScale = Vector3.Lerp(transform.localScale, target, Time.unscaledDeltaTime * scaleLerpSpeed);
        }
        
        private void ChangeSettings()
        {
            bool newSkin = GameSave.GetSettings().newSkin;
            _newSkin.SetActive(newSkin);
        }

        protected override void Click()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                GameManager.Instance.ActivateLevelScene(() =>
                {
                    GameManager.Instance.GameState = GameState.skin;
                    panelManager.OpenPanel(skinPanel);
                    GameManager.Instance.Action.ActivateSkinPanel?.Invoke(true);
                });
            }
            else
            {
                panelManager.OpenPanel(skinPanel);
                GameManager.Instance.GameState = GameState.skin;
            }
        }
    }
}