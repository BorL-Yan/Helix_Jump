using UnityEngine;
using VContainer;

public class CapsuleRotation : MonoBehaviour
{   
    [SerializeField] private float _interpolationRatio = 1f;
    [SerializeField] private float _rotationSpeed = 350f;
    [SerializeField] private Transform _transform;
    private Vector2 _direction;

    // private GameAction _action;
    //
    // [Inject]
    // public void Construct(GameAction action)
    // {
    //     _action = action;
    // } 
    
    private void OnMove(float direction)
    {
        _direction.x += direction * _rotationSpeed;
        
        //transform.Rotate(Vector3.up, -_direction.x * 0.1f);

        Quaternion quaternion = Quaternion.Euler(Vector3.up * -_direction.x);
        transform.rotation = Quaternion.Lerp(transform.rotation, quaternion, _interpolationRatio);
    }


    // private void OnEnable()
    // {
    //     _action.Move += OnMove;
    // }
    //
    // private void OnDisable()
    // {
    //     _action.Move -= OnMove;
    // }
}
