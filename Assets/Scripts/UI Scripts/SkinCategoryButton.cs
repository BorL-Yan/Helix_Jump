using UnityEngine;

public class SkinCategoryButton : UIButton
{
    [SerializeField] private ShopPanelManager panelManager;
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _active;
    [SerializeField] private GameObject _deactive;

    [SerializeField] private RectTransform _activePosition;
    
    protected override void Click()
    {
        panelManager.SelectCategory(this);
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            panelManager.transform.localPosition = _activePosition.localPosition;
        }
        _active.SetActive(value);
        _panel.SetActive(value);
        _deactive.SetActive(!value);
    }
    
    
    
    
}