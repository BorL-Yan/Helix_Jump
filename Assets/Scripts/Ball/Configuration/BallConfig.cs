using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ball Config", menuName = "Scriptable Objects/Ball Config")]
public class BallConfig : ScriptableObject
{
    [field: SerializeField] public float JumpHeight { get; private set; }= 4f;      
    [field: SerializeField, Min(0.0001f)] public float TimeToApex { get; private set; }= 0.4f;  
    [field: SerializeField] public float FallMultiplier { get; private set; } = 2f;   
    [field: SerializeField] public float MaxFallSpeed { get; private set; } = 20f;

    [field: SerializeField] public float RotationSpeed { get; private set; } = 20f;

    [field:SerializeField] public int MaxPlatformBreak { get; private set; } = 3; 

    [field:SerializeField] public float HangThreshold { get; private set;} = 0.5f; 
    [field:SerializeField] public float HangGravityMultiplier { get; private set;} = 0.1f;
    
    
    [SerializeField] private List<BallColor> _materials;
    public BallColor GetMaterial()
    {
        int level = GameSave.GetSettings().ActiveLevelSkin;
        
        return GetMaterial(level % _materials.Count);
    }

    private BallColor GetMaterial(int index)
    {
        if (_materials == null || index < 0 || index >= _materials.Count)
        {
            return _materials != null && _materials.Count > 0 ? _materials[0] : null;
        }
        return _materials[index];
    }
}
