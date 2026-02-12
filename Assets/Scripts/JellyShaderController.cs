using UnityEngine;

public class JellyShaderController : MonoBehaviour
{
    private Material material;

    void Start()
    {
        // Получаем материал (важно использовать .material, чтобы создать копию для этого объекта)
        material = GetComponent<Renderer>().material;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        
        if (collision.gameObject.CompareTag("Player"))
        {
            // Передаем точку касания
            material.SetVector("_ImpactPos", collision.contacts[0].point);
            // Передаем текущее время Unity
            material.SetFloat("_ImpactTime", Time.time);
        }
    }
}