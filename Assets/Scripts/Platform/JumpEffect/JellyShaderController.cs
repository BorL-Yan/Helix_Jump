using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class JellyShaderController : MonoBehaviour
{
    private static readonly int ImpactPosID = Shader.PropertyToID("_ImpactPos");
    private static readonly int ImpactTimeID = Shader.PropertyToID("_ImpactTime");

    [SerializeField] private float reactionRadius = 3f;

    private Renderer rend;
    private MaterialPropertyBlock block;
    private PlatformGroupJellyController _groupJellyController;

    public void Init(PlatformGroupJellyController controller)
    {
        _groupJellyController = controller;
    }
    private void Awake()
    {
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();

        // чтобы шейдер не срабатывал сразу при старте
        rend.GetPropertyBlock(block);
        block.SetFloat(ImpactTimeID, -999f);
        rend.SetPropertyBlock(block);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(!collider.CompareTag("Player")) return;
        Vector3 hitPoint = collider.transform.position;
        _groupJellyController.JellEffect(hitPoint);
    }

    public void ReceiveImpact(Vector3 point)
    {
        rend.GetPropertyBlock(block);

        block.SetVector(ImpactPosID, point);
        block.SetFloat(ImpactTimeID, Time.time);

        rend.SetPropertyBlock(block);
    }
}