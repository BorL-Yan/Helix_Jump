using UnityEngine;
public class CameraTriggerAttachment
{
    //Fixed positon Camera for y axis
    private Transform _mainCamera;
    private Transform _target;
    private float _deltaCameraTarget = 5.88f;
    private Vector3 _delta = Vector3.up;
    private Vector3 _tar;
    public CameraTriggerAttachment(Transform camera, Transform target)
    {        
        _target = target;
        _mainCamera = camera;
        _tar = _mainCamera.position;
        
    }
    public void Attach()
    {
        _delta.y = _deltaCameraTarget;
        _tar.y = _target.position.y;
        _mainCamera.transform.position = _tar + _delta;
        _mainCamera.SetParent(_target);      
    }
    public void Detach()
    {
        _mainCamera.SetParent(null);
    }
}
