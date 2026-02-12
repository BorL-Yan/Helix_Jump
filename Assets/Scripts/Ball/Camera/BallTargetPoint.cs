using UnityEngine;
using VContainer;

public class BallTargetPoint : MonoBehaviour
{
    [SerializeField] private Transform _ball;
    
    private LevelAction _levelAction;

    [Inject]
    public void Construct(LevelAction gameAction)
    {
        _levelAction = gameAction;
        _levelAction.OnStartLevel += SetTargetPos;
    }
    
    private void Update()
    {
        if (transform.position.y > _ball.position.y)
        {
            Vector3 currentPos = transform.position;
            currentPos.y = _ball.position.y;
            transform.position = currentPos;
        }
    }
    
    private void SetTargetPos()
    {
        Physics.Raycast(_ball.position, Vector3.down, out RaycastHit hit);
        transform.position = hit.point;
    }
    

    private void OnDestroy()
    {
        _levelAction.OnStartLevel -= SetTargetPos;
    }
    
}
