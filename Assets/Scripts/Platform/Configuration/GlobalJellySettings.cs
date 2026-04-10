using UnityEngine;

// Эта строчка добавит пункт в меню создания файлов (Right Click -> Create -> Settings -> Jelly Settings)
[CreateAssetMenu(fileName = "GlobalJellySettings", menuName = "Settings/Jelly Settings")]
public class GlobalJellySettings : ScriptableObject
{
    [Header("Jelly Physics")]
    public float degreeAttenuation = 1.5f;
    public float waveSpeed = 10f;
    public float waveAmplitude = 0.5f;
    public float stiffness = 2.0f;
    
    private void OnValidate()
    {
        ApplyToShader();
    }

    public void ApplyToShader()
    {
        Shader.SetGlobalFloat("_DegreeAttenuation", degreeAttenuation);
        Shader.SetGlobalFloat("_WaveSpeed", waveSpeed);
        Shader.SetGlobalFloat("_WaveAmplitude", waveAmplitude);
        Shader.SetGlobalFloat("_Stiffness", stiffness);
    }
}