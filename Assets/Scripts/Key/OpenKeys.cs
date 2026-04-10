using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class OpenKeys : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _openBox;
    [SerializeField] private UIButton _openButton;
    [SerializeField] private UIButton _nextButton;

    [SerializeField] private Animation _uiAnimation;
    [SerializeField] private GameObject _openBoxUI;

    private Action _callback;
    
    private void Awake()
    {
        _panel.SetActive(false);
        _openBox.SetActive(false);
        
        _openButton.gameObject.SetActive(false);
        _nextButton.gameObject.SetActive(false);
        _openBoxUI.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _nextButton.OnClick += Deactivate;
    }
    
    private void OnDisable()
    {
        _nextButton.OnClick -= Deactivate;
    }

    [VInspector.Button]
    public void Activate(Action callback)
    {
        _callback = callback;
        Sequence sequence = DOTween.Sequence();
        _panel.SetActive(true);
        sequence
            .AppendInterval(0.2f)
            .AppendCallback(() =>
                {
                    _animator.Play("Spawn");
                })
            .AppendInterval(1f)
            .AppendCallback(() =>
            {
                _openButton.gameObject.SetActive(true);
                _openButton.OnClick += OpenBox;
            });
    }

    private void Deactivate()
    {
        _panel.SetActive(false);
        _callback?.Invoke();
    }
    
    void OpenBox()
    {
        _animator.Play("StartOpen");
        
        _uiAnimation.Play("Close");
        _openButton.gameObject.SetActive(false);
    }

    public void BoxOpened()
    {
        _openBox.SetActive(true);
        _openBoxUI.SetActive(true);
        _nextButton.gameObject.SetActive(true);
        GameManager.Instance.Action.OnActivateNewSkin(BallSkinType.Hexagon);
        GameSave.GetSettings().newSkin = true;
        GameSave.GetSettings().ActiveSkins[BallSkinType.Hexagon] = true;
        GameSave.Save();
    }
}
