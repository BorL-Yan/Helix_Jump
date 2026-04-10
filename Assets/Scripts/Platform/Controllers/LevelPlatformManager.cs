using Ball.Controller;
using com.cyborgAssets.inspectorButtonPro;
using Level.Controllers;
using UnityEngine;
using VContainer;


public class LevelPlatformManager : MonoBehaviour
{
    [field: SerializeField] public int PlatformsCount { get; private set; }
    [SerializeField] private GameObject _root;
    private LevelAction _levelAction;
    [SerializeField] private GlobalJellySettings _globalJellySettings;
    [Inject]
    public void Construct(LevelAction levelAction)
    {
        _levelAction = levelAction;
    }
    

    public void Initialize(int platformID)
    {
        _globalJellySettings.ApplyToShader();
        GameObject level = Resources.Load<GameObject>("Levels/Level_" + platformID);
        if(level != null)
             level = Instantiate(level, _root.transform);
        
        level.GetComponentInChildren<FinishPlatform>().Init(_levelAction);
        GetComponent<PlatformsColor>().SetMaterial();
        UpdatePlatformCount();
    }
    
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
