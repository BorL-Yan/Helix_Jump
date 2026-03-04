using Level.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI_Scripts
{
    public class LeauguePanelController:UIButton
    {
            [SerializeField] private PanelManager panelManager;
            [SerializeField] private LevelLoadingUI levelLoadingUI;
            [Header("Highlight")]
            [SerializeField] private float normalScale = 1f;
            [SerializeField] private float openedPanelScale = 1.1f;
            [SerializeField] private float scaleLerpSpeed = 10f;

            [Header("Panel")]
            [SerializeField] private GameObject LeauguePanel;

            private void Start()
            {
                transform.localScale = Vector3.one * normalScale;
            }

            private void Update()
            {
                if (LeauguePanel == null)
                    return;

                float targetScale = LeauguePanel.activeSelf ? openedPanelScale : normalScale;
                var target = Vector3.one * targetScale;
                transform.localScale = Vector3.Lerp(transform.localScale, target, Time.unscaledDeltaTime * scaleLerpSpeed);
            }

            protected override void Click()
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    levelLoadingUI.ActivateLevel(1);
                }

                panelManager.OpenPanel(LeauguePanel);
            }
    }
}