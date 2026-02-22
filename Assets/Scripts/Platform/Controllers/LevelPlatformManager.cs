using System;
using com.cyborgAssets.inspectorButtonPro;
using Level.Controllers;
using UnityEditor;
using UnityEngine;
using VContainer;


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
