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

    
    [SerializeField] private List<Material> _materials;
    public Material GetMaterial()
    {
        //TODO GameManger.CurrentActiveLevel
        
        return _materials[0];
    }
    
}
