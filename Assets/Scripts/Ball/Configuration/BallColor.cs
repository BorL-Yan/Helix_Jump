using UnityEngine;

[System.Serializable]
public class BallColor
{
    [field: SerializeField] public Material Material { get; private set; }
    [field: SerializeField] public Gradient Gradient { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
}
