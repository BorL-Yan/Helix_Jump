using UnityEngine;

namespace Platform.Main_Scene
{
    public class PlatformClamp : MonoBehaviour
    {
        [SerializeField] private float minZ;
        [SerializeField] private float maxZ;
        [SerializeField] private float speed = 2f;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            float move = Input.GetAxis("Vertical") * speed;
            Vector3 pos = rb.position;
            
            pos.z += move * Time.fixedDeltaTime;
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

            rb.MovePosition(pos);
        }
    }
}