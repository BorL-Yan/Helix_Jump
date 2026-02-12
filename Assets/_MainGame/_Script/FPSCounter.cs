using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    float fps;
    void Update()
    {
        fps = 1f / Time.deltaTime;
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 40;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(20, 20, 200, 50), "FPS: " + Mathf.RoundToInt(fps), style);
    }
}