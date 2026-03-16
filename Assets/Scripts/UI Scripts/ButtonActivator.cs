using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class ButtonActivator : MonoBehaviour
{
    [Header("Animation Scripts")]
    [SerializeField] private PulseUI[] pulseAnimations;
    [SerializeField] private RotateUI[] rotateAnimations;

    [Header("Scale Settings")]
    [SerializeField] private float activeScale = 1.2f;
    [SerializeField] private float scaleDuration = 0.25f;

    private RectTransform rect;
    private Vector3 normalScale;

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

        EnableAnimations();

        rect.DOScale(activeScale, scaleDuration)
            .SetEase(Ease.OutBack);
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
                pulse.enabled = true;

        foreach (var rotate in rotateAnimations)
            if (rotate != null)
                rotate.enabled = true;
    }

    private void DisableAnimations()
    {
        foreach (var pulse in pulseAnimations)
            if (pulse != null)
                pulse.enabled = false;

        foreach (var rotate in rotateAnimations)
            if (rotate != null)
                rotate.enabled = false;
    }
}