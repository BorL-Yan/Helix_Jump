using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LevelProgress : MonoBehaviour
{
    [SerializeField] private GameObject _progressPanel;
    
    [SerializeField] private TMP_Text _currentLevel;
    private int currentLevel;
    [SerializeField] private TMP_Text _nextLevel;

    [SerializeField] private TMP_Text _pointText;
    private int point;
    [SerializeField] private TMP_Text _pointAnimationText;
    [SerializeField] private RectTransform _rectTransform;
    
    [SerializeField] private Image _levelProgress;
    private int platformsCount;
    private int currentPlatformsCount;

    private LevelAction _levelAction;
    private Sequence progressSequence;

    [Inject]
    public void Construct(LevelAction levelAction)
    {
        _levelAction = levelAction;
    }
    
    private void Start()
    {
        _progressPanel.SetActive(true);
        _levelProgress.fillAmount = 0;
        point = 0;
        currentPlatformsCount = 0;
        _pointAnimationText.gameObject.SetActive(false);
    }

    public void Activate(int level, int platformscount)
    {
        currentLevel = level;
        _currentLevel.text = level.ToString();
        _nextLevel.text = (level+1).ToString();
        platformsCount = platformscount;
    }
    
    
    public void SetPoint(int value)
    {
        point += value * currentLevel;
        _pointText.text = NumberFormatter.FormatValue(point);
        currentPlatformsCount++;
        progressSequence?.Kill();
        progressSequence = DOTween.Sequence();
        progressSequence.Append(_levelProgress.DOFillAmount((float)currentPlatformsCount / platformsCount, 0.4f));
    }
    
    public int GetPoint() => point;

    public void ActivateAnimationPoint(int value, Action callback)
    {
        int _displayScore = point;
        point *= value;
        

        DOTween.To(() => _displayScore, x => _displayScore = x, point, 1f)
            .OnUpdate(() =>
            {
                _pointText.text = NumberFormatter.FormatValue(_displayScore);
            })
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                callback?.Invoke();
            });
    }

    public void SetPointAnimation(int value)
    {
        _pointAnimationText.gameObject.SetActive(true);
        SetPoint(value);
        _pointAnimationText.text = "+" + NumberFormatter.FormatValue(value * currentLevel);
        
        Sequence sequence = DOTween.Sequence();
        _pointAnimationText.rectTransform.localPosition = _rectTransform.localPosition;

        Color color = _pointAnimationText.color * new Color(1, 1, 1, 0);
        _pointAnimationText.color = new Color(color.r, color.g, color.b, 1);

        sequence.Append(_pointAnimationText.transform.DOLocalMoveY(_pointText.transform.localPosition.y, 1f))
            .Join(_pointAnimationText.DOColor(color, 1f))
            .OnComplete(() =>
            {
                _pointAnimationText.gameObject.SetActive(false);
            });
    }
    
    
    private void Deactivate()
    {
        _progressPanel.SetActive(false);
    }

    private void OnEnable()
    {
        _levelAction.OnFinishLevel += Deactivate;
        _levelAction.OnSetPoint += SetPoint;
        _levelAction.OnPointAnimation += SetPointAnimation;
    }

    private void OnDisable()
    {
        _levelAction.OnFinishLevel -= Deactivate;
    }
}
