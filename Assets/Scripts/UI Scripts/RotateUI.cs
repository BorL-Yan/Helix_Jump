using UnityEngine;
using DG.Tweening;

public class RotateUI : MonoBehaviour
{
    [SerializeField] private float rotationAmount = 360f; // քանի աստիճան պտտվի
    [SerializeField] private float duration = 2f;         // քանի վայրկյանում

    private void Start()
    {
        transform
            .DORotate(new Vector3(0, 0, rotationAmount), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
}