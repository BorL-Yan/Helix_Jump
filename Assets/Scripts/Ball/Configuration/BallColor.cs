using UnityEngine;

[CreateAssetMenu(fileName = "Ball Color", menuName = "Scriptable Objects/Ball Color")]
public class BallColor : ScriptableObject
{
    [field: SerializeField] public Material BallMaterial { get; private set; }
    [field: SerializeField] public Material ParticleMaterial { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
}
