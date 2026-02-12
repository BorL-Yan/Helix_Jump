using UnityEngine;

namespace Ball.Camera
{
    public class CameraTarget : MonoBehaviour
    {
        [SerializeField] private Vector3 _targetOffset;
        [SerializeField] private Transform _targetPoint;
        
        private void LateUpdate()
        {
            if (transform.position.y >= _targetPoint.position.y + _targetOffset.y)
            {
                transform.position = _targetPoint.position + _targetOffset;
            }
        }
    }
}