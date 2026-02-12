using UnityEngine;
public class RotationBallRenderer : MonoBehaviour 
{
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private float angularDrag = 2f;

    private Vector3 angularVelocity;

    public void jumprotation()
    {
        angularVelocity = Random.onUnitSphere * rotationSpeed;
    }

    void Update()
    {
        // вращаем
        transform.Rotate(angularVelocity * Time.deltaTime, Space.Self);

        // затухание (НЕ зависит от FPS)
        angularVelocity = Vector3.Lerp(
            angularVelocity,
            Vector3.zero,
            angularDrag * Time.deltaTime
        );
    }

}
