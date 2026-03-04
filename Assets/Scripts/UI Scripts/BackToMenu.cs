using UnityEngine;

public class BackToMenu : UIButton
{
    [SerializeField] private PanelManager panelManager;
    [Header("Highlight")]
    [SerializeField] private float normalScale = 1f;
    [SerializeField] private float clickedScale = 1.1f;

    private void Start()
    {
        if (GameManager.Instance.IsMenuActive)
            transform.localScale = Vector3.one * clickedScale;
        else
            transform.localScale = Vector3.one * normalScale;
    }

    protected override void Click()
    {
        transform.localScale = Vector3.one * clickedScale;
        panelManager.CloseAllPanels();
        GameManager.Instance.ActivateMenuScene();
    }
}