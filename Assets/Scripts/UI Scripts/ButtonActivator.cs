using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class ButtonActivator : MonoBehaviour
{
    [Header("Animation Scripts")]
    [SerializeField] private PulseUI[] pulseAnimations;
    [SerializeField] private RotateUI[] rotateAnimations;

    [Header("Scale Settings")]
    [SerializeField] private float activeScale = 1.2f;
    [SerializeField] private float scaleDuration = 0.25f;

    [SerializeField] private int RankedID;
    
    private RectTransform rect;
    private Vector3 normalScale;

    [SerializeField] private Image _spriteRenderer;
    [SerializeField] private Sprite _active;
    
    public bool IsActive { get; private set; }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        normalScale = rect.localScale;

        DisableAnimations();
        Activate();
    }

    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;

        int currentRankedID = GameSave.GetSettings().RankedID;

        if (currentRankedID - 1 == RankedID)
        {
            _spriteRenderer.sprite = _active;
        }
        
        if (currentRankedID == RankedID)
        {
            EnableAnimations();
            rect.DOScale(activeScale, scaleDuration)
                .SetEase(Ease.OutBack);
        }
        else
        {
            Deactivate();
        }

    }

    public void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;

        DisableAnimations();

        rect.DOScale(normalScale, scaleDuration)
            .SetEase(Ease.InOutSine);
    }

    private void EnableAnimations()
    {
        foreach (var pulse in pulseAnimations)
            if (pulse != null)
            {
                pulse.enabled = true;
                pulse.gameObject.SetActive(true);
            }

        foreach (var rotate in rotateAnimations)
            if (rotate != null)
            {
                rotate.enabled = true;
                rotate.gameObject.SetActive(true);
            }
    }

    private void DisableAnimations()
    {
        foreach (var pulse in pulseAnimations)
            if (pulse != null)
            {
                pulse.enabled = false;
                pulse.gameObject.SetActive(false);
            }

        foreach (var rotate in rotateAnimations)
            if (rotate != null)
            {
                rotate.enabled = false;
                rotate.gameObject.SetActive(false);
            }
    }
}