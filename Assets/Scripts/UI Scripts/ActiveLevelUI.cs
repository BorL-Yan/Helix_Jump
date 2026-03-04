using UnityEngine;
using DG.Tweening; // Անպայման ավելացրու սա

public class ActiveLevelUI : UIButton
{
    private Tween _vibrationTween;

    protected override void Click()
    {
        GameManager.Instance.Action.MoveToActivePlatform?.Invoke();
    }

    private void OnEnable()
    {
        StartVibration();
    }

    private void OnDisable()
    {
        StopVibration();
    }

    public void SetDirection(bool isUp)
    {
        float angle = isUp ? 180f : 0f;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
    
    private void StartVibration()
    {
        StopVibration();

        transform.localScale = new Vector3(1f, 1f, 1f);

        _vibrationTween = transform
            .DOScaleY(1.05f, 0.3f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void StopVibration()
    {
        if (_vibrationTween != null && _vibrationTween.IsActive())
        {
            _vibrationTween.Kill();
        }
        transform.localScale = Vector3.one;
    }
}