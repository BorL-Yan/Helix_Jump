using UnityEngine;

public class PlatformGroupJellyController : MonoBehaviour
{
    private JellyShaderController[] _shaderControllers;
    
    private void Awake()
    {
        _shaderControllers = transform.GetComponentsInChildren<JellyShaderController>();
        for (int i = 0; i < _shaderControllers.Length; i++)
        {
            _shaderControllers[i].Init(this);
        } 
    }
    
    public void JellEffect(Vector3 pos)
    {
        for (int i = 0; i < _shaderControllers.Length; i++)
        {
            _shaderControllers[i].ReceiveImpact(pos);
        }   
    }
}