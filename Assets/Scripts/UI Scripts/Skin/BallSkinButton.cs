using DG.Tweening;
using UnityEngine;

public class BallSkinButton : UIButton
{
    [field: SerializeField] public BallSkinType SkinID { get; private set; }
    [SerializeField] private SkinManager _basicSkinManager;

    [SerializeField] private GameObject _active;
    [SerializeField] private GameObject _select;
    [SerializeField] private GameObject _deactive;

    [SerializeField] private GameObject _buyIcon;

    public bool isActive { get; private set; }
    private bool _isSelect;

    private void OnEnable()
    {
        Sequence sequence = DOTween.Sequence();
        transform.localScale = Vector3.one * 0.6f;
        sequence.Append(transform.DOScale(1.2f, 0.2f))
            .Append(transform.DOScale(1, 0.08f));
    }
    
    
    protected override void Click()
    {
        if(isActive && !_isSelect) Select();
    }

    protected override void ButtonAnimation()
    {
        Click();
    }

    public void Activate()
    {
        isActive = true;
        _isSelect = false;
        Active(_active);
    }

    public void Select()
    {
        _isSelect = true;
        isActive = true;
        _basicSkinManager.SelectSkin(this);
        Active(_select);
        _active.SetActive(true);
        GameManager.Instance.Action.OnSelectBallSkin?.Invoke(SkinID);
        Debug.Log($"Select {gameObject.name}: Skin {SkinID}");
    }

    public void Deactivate()
    {
        isActive = false;
        _isSelect = false;
        Active(_deactive);
    }

    public void BuyIcon(bool value)
    {
        _buyIcon.SetActive(value);
    }
    
    private void Active(GameObject obj)
    {
        if(_active != null)
            _active.SetActive(obj == _active);
        if(_select != null)
            _select.SetActive(obj == _select);
        if(_deactive != null)
            _deactive.SetActive(obj == _deactive);
    }
    
}