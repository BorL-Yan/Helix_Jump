using System;
using UnityEditor;
using UnityEngine;

public class JellyShaderGUI : ShaderGUI
{
    // Ссылка на стандартный инспектор URP Lit
    private ShaderGUI _litGui;
    private bool _jellyFoldoutOpen = true;
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        // 1. Пытаемся найти и создать стандартный инспектор LitShader через рефлексию
        if (_litGui == null)
        {
            Type litType = Type.GetType("UnityEditor.Rendering.Universal.ShaderGUI.LitShader, Unity.RenderPipelines.Universal.Editor");
            if (litType != null)
            {
                _litGui = (ShaderGUI)Activator.CreateInstance(litType);
            }
        }

        // 2. Отрисовываем стандартный интерфейс (если нашли) или базовый
        if (_litGui != null)
        {
            _litGui.OnGUI(materialEditor, properties);
        }
        else
        {
            base.OnGUI(materialEditor, properties); // Запасной вариант
        }

        // 3. Добавляем визуальный разделитель
        EditorGUILayout.Space(15);

        _jellyFoldoutOpen = EditorGUILayout.BeginFoldoutHeaderGroup(_jellyFoldoutOpen, "Jelly Settings");

        if (_jellyFoldoutOpen)
        {
            DrawProperty("_DegreeAttenuation", "Degree Attenuation", properties, materialEditor);
            DrawProperty("_WaveSpeed", "Wave Speed", properties, materialEditor);
            DrawProperty("_WaveAmplitude", "Wave Amplitude", properties, materialEditor);
            DrawProperty("_Stiffness", "Stiffness", properties, materialEditor);
            DrawProperty("_CubeSize", "Cube Size", properties, materialEditor);
            
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    // Вспомогательный метод для удобной отрисовки
    private void DrawProperty(string propertyName, string label, MaterialProperty[] properties, MaterialEditor materialEditor)
    {
        MaterialProperty prop = FindProperty(propertyName, properties, false);
        if (prop != null)
        {
            materialEditor.ShaderProperty(prop, new GUIContent(label));
        }
    }
}