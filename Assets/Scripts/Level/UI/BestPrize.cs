using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Level;
using TMPro;
using UI_Scripts.Bonus_UI;
using UnityEngine;
using Random = UnityEngine.Random;


public class BestPrize : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private UIButton _closeButton;
    [SerializeField] private GameObject _adsButton;
    [SerializeField] private GameObject[] _keys;

    [SerializeField] private TMP_Text _pointText;
    
    private int currentKeys;
    [SerializeField] private CoinUIAnimation _coinUIAnimation;
    [SerializeField] private List<ChestButton> _chestButtons;
    
    private readonly int[] prizeMoney = new[] { 50, 75, 150 };
    
    private Action _callback;
    private Coroutine _chestCoroutine;

    private void Awake()
    {
        var childs = GetComponentsInChildren<ChestButton>(true);
        _chestButtons = new List<ChestButton>(childs);
    }

    private void Start()
    {
        _panel.SetActive(false);
    }
    
    [VInspector.Button]
    public void Activate(Action callback)
    {
        _callback = callback;
        _panel.SetActive(true);
        _chestButtons.ForEach(item =>
        {
            item.SetBestPrizeComponent(this);
            item.SpawnChest();
        });
        _chestCoroutine = StartCoroutine(RotateChest());
        _pointText.text = NumberFormatter.FormatValue(GameSave.GetSettings().Coin);
        
        currentKeys = 3;
        ActivateCloseButton(false);
        SoundManager.Instance.Play(SoundType.Open_Chests_Panel);
    }

    public void ActivateCloseButton(bool value)
    {
        _adsButton.SetActive(value);
        _closeButton.gameObject.SetActive(value);
    }

    public void OpenChest(Action<BestPrizeType> callback, ChestButton chest)
    {
        currentKeys--;
        if (currentKeys < 0)
        {
            callback?.Invoke(BestPrizeType.Null);
            Debug.Log("Return");
            return;
            
        }
        if (currentKeys == 0)
        {
            ActivateCloseButton(true);
        }

        int money = 0;
        GameObject keyObj = _keys[currentKeys];
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(keyObj.transform.DOScale(0, 0.5f))
            .AppendCallback(() =>
            {
                chest.OpenChest();
            })
            .AppendInterval(0.2f)
            .AppendCallback(() =>
            {
                _coinUIAnimation.ActivateAnimation(null);
                int randType = Random.Range(0, 3);
                BestPrizeType prizeType = (BestPrizeType)randType;

                money = prizeMoney[randType];
                callback?.Invoke(prizeType);
            }).AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                var save = GameSave.GetSettings();

                _pointText.text = NumberFormatter.FormatValue(save.Coin);

                int currentPoint = save.Coin;

                int point = currentPoint + money;
                DOTween.To(() => currentPoint, x => currentPoint = x, point, 0.8f)
                    .OnUpdate(() =>
                    {
                        _pointText.text = NumberFormatter.FormatValue(currentPoint);
                    })
                    .SetEase(Ease.Linear);
                save.Coin += money;
                GameSave.SetSettings(save);  
            });

    }
    
    private IEnumerator RotateChest()
    {
        var timeYield = new WaitForSeconds(2f);
        var minYield = new WaitForSeconds(0.1f);
        while (true)
        {
            yield return timeYield;
            foreach (var chest in _chestButtons)
            {
                chest.RotateChest();
                yield return minYield;
            }
        }
    }

    private void Deactivate()
    {
        _callback?.Invoke();
        _panel.SetActive(false);
        StopCoroutine(_chestCoroutine);
    }
    
    private void OnEnable()
    {
        _closeButton.OnClick += Deactivate;
    }

    private void OnDisable()
    {
        _closeButton.OnClick -= Deactivate;
    }
    
}

public enum BestPrizeType : byte { Low = 0, Middle, Height, Null }