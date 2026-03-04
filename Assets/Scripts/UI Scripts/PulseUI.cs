using UnityEngine;
using DG.Tweening;

public class PulseUI : MonoBehaviour
{
    public enum Direction
    {
        Right,
        Left
    }

    [Header("Movement Settings")]
    [SerializeField] private Direction direction = Direction.Right;
    [SerializeField] private float moveAmount = 10f;  
    [SerializeField] private float duration = 0.6f;

    private RectTransform rect;
    private float startX;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        startX = rect.anchoredPosition.x;

        float targetX = direction == Direction.Right
            ? startX + moveAmount
            : startX - moveAmount;

        rect.DOAnchorPosX(targetX, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}