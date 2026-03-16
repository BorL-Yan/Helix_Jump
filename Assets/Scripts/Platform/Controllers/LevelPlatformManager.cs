using com.cyborgAssets.inspectorButtonPro;
using Level.Controllers;
using Platform.Controllers;
using UnityEditor;
using UnityEngine;


public class LevelPlatformManager : MonoBehaviour
{
    [field: SerializeField] public int PlatformsCount { get; private set; }

    private void OnValidate()
    {
        UpdatePlatformCount();
    }
        
    [ProButton]
    private void Reset()
    {
        UpdatePlatformCount();
        
    }

    private void UpdatePlatformCount()
    {
        var children = GetComponentsInChildren<PlatformController>(true);
        PlatformsCount = children.Length;
    }
}
