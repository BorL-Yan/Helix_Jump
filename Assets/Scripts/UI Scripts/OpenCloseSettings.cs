using UnityEngine;

public class OpenCloseSettings : UIButton
{
    [Header("Settings")]
    //[SerializeField] private ActionType actionType;
    [SerializeField] private GameObject settingsPanel;
    private bool active;
    protected override void Click()
    {
        SetActive(!active);
    }

    void Start()
    {
        SetActive(false);
    }
    private void SetActive(bool active)
    {
        this.active = active;
        settingsPanel.SetActive(active);
    }
    public enum ActionType
    {
        Open,
        Close
    }
}