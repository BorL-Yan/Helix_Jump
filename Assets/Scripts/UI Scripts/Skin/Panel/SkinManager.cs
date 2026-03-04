using System;
using System.Collections.Generic;
using System.Linq;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UI_Scripts.Skin;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using VContainer;
using Random = UnityEngine.Random;


public abstract class SkinManager : MonoBehaviour
{
    [SerializeField] protected List<BallSkinButton> _skins;
    [SerializeField] protected BuySkin _buySkin;

    private BallSkinType activeSkin;
    private BallSkinButton _activeSkinButton;
    
    private void OnEnable()
    {
        LoadActiveSkin();
    }
    
    public void SelectSkin(BallSkinButton skinButton)
    {
        activeSkin = skinButton.SkinID;
        _activeSkinButton?.Activate();
        _activeSkinButton = skinButton;
        SaveSkin(activeSkin);
    }

    private void UpdateUI()
    {
        var settings = GameSave.GetSettings();
        foreach (var skin in _skins)
        {
            if (settings.ActiveSkins.TryGetValue(skin.SkinID, out var value))
            {
                if (skin.SkinID == activeSkin)
                {
                    _activeSkinButton = skin;
                    skin.Select();
                }
                else if (value)
                {
                    skin.Activate();
                }
                else
                {
                    skin.Deactivate();
                }
            }
            else
            {
                settings.ActiveSkins.TryAdd(skin.SkinID, false);
                skin.Deactivate();
            }
        }
        _buySkin?.UpdatePrice();
    }

    private void LoadActiveSkin()
    {
        var settings = GameSave.GetSettings();
        activeSkin = settings.ActiveBallSkin;
     
        UpdateUI();
    }
    private void SaveSkin(BallSkinType type)
    {
        var settings = GameSave.GetSettings();
        settings.ActiveBallSkin = type;
        settings.ActiveSkins[type] = true;
        
        GameSave.SetSettings(settings);
        GameSave.Save();
    }
    
    
    private void OnValidate()
    {
        SetSkins();
    }

    private void Reset()
    {
        SetSkins();
    }


    public int GetActiveSkinCount()
    {
        return _skins.FindAll(x => x.isActive).Count;
    } 

    [ProButton]
    public void SelectAnimation(Action callback)
    {
        var deactiveSkins = _skins.Where(item => !item.isActive)
            .OrderBy(x => UnityEngine.Random.value).ToList();
        
        var isActivateSkin = deactiveSkins.Find(item => item.SkinID != BallSkinType.Null);
        int randomvalue = Random.Range(20, 30);
        BallSkinButton skin = deactiveSkins[0];
        
        Sequence sequence = DOTween.Sequence();
        
        for (int i = 0; i <= randomvalue; i++)
        {
            float duration = Mathf.Lerp(0.1f, 0.3f, (float)i / randomvalue);
            int index = i % deactiveSkins.Count;

            sequence.AppendCallback(() =>
                {
                    skin?.BuyIcon(false);
                    skin = deactiveSkins[index];
                    skin?.BuyIcon(true);
                })
                .AppendInterval(duration);
        }

        sequence.AppendCallback(() =>
        {
            skin.BuyIcon(false);
            _activeSkinButton.Activate();
            isActivateSkin.BuyIcon(true);
            activeSkin = isActivateSkin.SkinID;
            GameManager.Instance.Action.OnSelectBallSkin?.Invoke(activeSkin);
            
            UpdateUI();
            callback?.Invoke();
        });
    }
    
    
    [ProButton]
    public void SetSkins()
    {
        var skinChilde = gameObject.GetComponentsInChildren<BallSkinButton>();
        _skins = skinChilde.ToList();
    }
    
}
