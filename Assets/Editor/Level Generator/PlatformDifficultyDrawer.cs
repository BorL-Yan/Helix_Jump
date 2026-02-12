using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PlatformDifficultyCurve))]
public class PlatformDifficultyDrawer : PropertyDrawer
{
    private const float RightPanelWidth = 100; // Ширина панели управления справа
    private const float Padding = 20f;          // Отступ
    private const float CurveHeight = 180f;     // Высота окна кривой
    private const float PropertivHeight = 70f;
    
    private const float RowSpacing = 2f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // 1. Заголовок переменной
        Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, label, EditorStyles.boldLabel);
        
        // Область контента ниже заголовка
        Rect contentRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 5, position.width, CurveHeight);

        // Разделяем на Лево (Кривая) и Право (Настройки)
        Rect curveRect = new Rect(contentRect.x, contentRect.y, contentRect.width, contentRect.height);
        Rect toolsRect = new Rect(contentRect.x, contentRect.y, RightPanelWidth, contentRect.height);

        // Свойства
        SerializedProperty curveProp = property.FindPropertyRelative("curve");
        SerializedProperty medProp = property.FindPropertyRelative("mediumThreshold");
        SerializedProperty hardProp = property.FindPropertyRelative("hardThreshold");
        SerializedProperty medColorProp = property.FindPropertyRelative("mediumColor");
        SerializedProperty hardColorProp = property.FindPropertyRelative("hardColor");

        // --- ЛЕВАЯ ЧАСТЬ: КРИВАЯ ---
        GUI.Box(curveRect, GUIContent.none, EditorStyles.helpBox);
        
        // Правильный вызов CurveField для одной кривой
        EditorGUI.CurveField(curveRect, curveProp, Color.green, new Rect(0, 0, 1, 1));

        // Отрисовка линий порогов ТОЛЬКО в инспекторе
        if (Event.current.type == EventType.Repaint)
        {
            DrawThresholdLine(curveRect, medProp.floatValue, medColorProp.colorValue, "Medium");
            DrawThresholdLine(curveRect, hardProp.floatValue, hardColorProp.colorValue, "Hard");
        }

        // --- ПРАВАЯ ЧАСТЬ: СЛАЙДЕРЫ ---
        float prevLabelWidth = EditorGUIUtility.labelWidth;
        int prevIndent = EditorGUI.indentLevel;

        // Минимальный отступ для текста, чтобы не было дыры перед цифрами
        EditorGUIUtility.labelWidth = 35f; 
        EditorGUI.indentLevel = 0;

        float lineH = EditorGUIUtility.singleLineHeight;

        // Рассчитываем позиции снизу вверх
        // 1. Кнопка (самый низ)
        Rect btnRect = new Rect(toolsRect.x, toolsRect.yMax - lineH + PropertivHeight, toolsRect.width , lineH);
        
        // 2. Поле Med (сразу над кнопкой)
        Rect medRect = new Rect(btnRect.x, btnRect.y - lineH - RowSpacing, toolsRect.width + 200, lineH);
        
        // 3. Поле Hard (сразу над Med)
        Rect hardRect = new Rect(medRect.x, medRect.y - lineH - RowSpacing, toolsRect.width + 200, lineH);

        // Отрисовка элементов
        if (GUI.Button(btnRect, "Random Colors", EditorStyles.miniButton))
        {
            medColorProp.colorValue = Random.ColorHSV(0f, 1f, 0.6f, 0.9f, 0.7f, 1f);
            hardColorProp.colorValue = Random.ColorHSV(0f, 1f, 0.6f, 0.9f, 0.7f, 1f);
        }

        medProp.floatValue = EditorGUI.Slider(medRect, "Med", medProp.floatValue, 0f, hardProp.floatValue);
        hardProp.floatValue = EditorGUI.Slider(hardRect, "Hard", hardProp.floatValue, medProp.floatValue, 1f);

        // Возврат настроек редактора
        EditorGUIUtility.labelWidth = prevLabelWidth;
        EditorGUI.indentLevel = prevIndent;

        EditorGUI.EndProperty();
    }

    private void DrawThresholdLine(Rect rect, float yValue, Color color, string label)
    {
        float pixelY = rect.y + (rect.height * (1f - yValue));
        if (pixelY < rect.y || pixelY > rect.yMax) return;

        Handles.BeginGUI();
        Handles.color = color;
        Handles.DrawLine(new Vector2(rect.x, pixelY), new Vector2(rect.x + rect.width, pixelY), 1f);
        Handles.EndGUI();

        GUIStyle style = new GUIStyle(EditorStyles.miniLabel) { normal = { textColor = color } };
        GUI.Label(new Rect(rect.x + 3, pixelY - 14, 40, 14), label, style);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return CurveHeight + EditorGUIUtility.singleLineHeight + PropertivHeight;
    }
}