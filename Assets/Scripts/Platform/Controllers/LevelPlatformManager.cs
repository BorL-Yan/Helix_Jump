using com.cyborgAssets.inspectorButtonPro;
using Level.Controllers;
using UnityEngine;


public class LevelPlatformManager : MonoBehaviour
{
    [field: SerializeField] public int PlatformsCount { get; private set; }
    private GameObject effect;
    
    private void OnValidate()
    {
        UpdatePlatformCount();
    }
        
    [ProButton]
    private void Reset()
    {
        UpdatePlatformCount();
        if (effect == null)
        {
            effect = Resources.Load<GameObject>("Partical/Finish");
            Instantiate(effect, transform);
        }
    }

    private void UpdatePlatformCount()
    {
        var children = GetComponentsInChildren<PlatformController>(true);
        PlatformsCount = children.Length;
    }
}
