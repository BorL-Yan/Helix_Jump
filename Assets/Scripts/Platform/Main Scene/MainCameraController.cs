using UnityEngine;

namespace Platform.Main_Scene
{
    public class MainCameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 _targetPosOffset;
        [SerializeField] private float _speed;
        [SerializeField] private float _speedSmooth;
        
        private Vector3 _targetPos;
        private void Start()
        {
            GameManager.Instance.Action.MoveY += Move;
            _targetPos = _targetPosOffset;
        }
        
        private void OnDestroy()
        {
            if(GameManager.Instance != null)
                GameManager.Instance.Action.MoveY -= Move;
        }
        
        private void Move(float y)
        {
            _targetPos = new Vector3(_targetPosOffset.x,
                _targetPosOffset.y,  
                -y*_speed + _targetPos.z);
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, _targetPos, _speedSmooth * Time.deltaTime);
        }
    }
}