using UnityEngine;

namespace Platform.Configuration
{
    [CreateAssetMenu(fileName = "Level Color", menuName = "Helix/Level Color", order = 1)]
    public class LevelColor: ScriptableObject
    {
        [field : SerializeField] public Sprite BackGround {get; private set;}
        [field : SerializeField] public Material Platform {get; private set;}
        [field : SerializeField] public Material Cilinder {get; private set;}
    }
}