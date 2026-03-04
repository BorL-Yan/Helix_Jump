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

        protected override void Click()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                GameManager.Instance.ActivateLevelScene(() => { panelManager.OpenPanel(skinPanel);});
            }
            else
            {
                panelManager.OpenPanel(skinPanel);
            }
        }
    }
}