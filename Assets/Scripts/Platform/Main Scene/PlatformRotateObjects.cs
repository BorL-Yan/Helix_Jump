using UnityEngine;

namespace Platform.Main_Scene
{
    public class PlatformRotateObjects : MonoBehaviour
    {
        [SerializeField] private GameObject _rotateObjLeft;
        [SerializeField] private GameObject _rotateObjRight;
        [SerializeField] private float _rotationSpeed;
        
        private void Update()
        {
            var rotation = _rotateObjLeft.transform.eulerAngles;
            rotation.y += _rotationSpeed * Time.deltaTime;
            _rotateObjLeft.transform.eulerAngles = rotation;
            
            rotation =  _rotateObjRight.transform.eulerAngles;
            rotation.y -= _rotationSpeed * Time.deltaTime;
            _rotateObjRight.transform.eulerAngles = rotation;
        }
    }
}