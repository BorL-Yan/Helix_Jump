using Platform.Configuration;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class PlatformsColor : MonoBehaviour
{
    [SerializeField] private Renderer _cilinder;
    [SerializeField] private Image _backGround;
    private LevelColorCollection _colorCollection;
    
    
    [Inject]
    public void Construct(LevelColorCollection colorCollection)
    {
        _colorCollection = colorCollection;
       
    }
    
    [VInspector.Button]
    public void SetMaterial()
    {
        LevelColor color = _colorCollection.GetColor();

        _cilinder.material = color.Cilinder;
        _backGround.sprite = color.BackGround;
        Material material = color.Platform;
        if (material == null) return;
        
        Renderer[] platforms = GetComponentsInChildren<Renderer>(true);
        foreach (var platform in platforms)
        {         
            if (platform.CompareTag("Platform"))
            {
                platform.material = material;
            }
        }
    }
}
