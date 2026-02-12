using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ComplexityCurve))]
public class ComplexityCurveDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // 1. Рисуем заголовок группы
        position = EditorGUI.PrefixLabel(position, label);

        SerializedProperty curveProp = property.FindPropertyRelative("curve");
        SerializedProperty levelsProp = property.FindPropertyRelative("levels");

        // 2. Настройка области для кривой
        Rect curveRect = new Rect(position.x, position.y, position.width, 100); // Высота 100px
        
        // Рисуем кривую, зажатую в диапазоне 0-1 по обеим осям
        curveProp.animationCurveValue = EditorGUI.CurveField(
            curveRect, 
            curveProp.animationCurveValue, 
            Color.green, 
            new Rect(0, 0, 1, 1) // Это ограничивает просмотр сегментом 0-1
        );

        // 3. Рисуем динамические линии сложности
        if (Event.current.type == EventType.Repaint)
        {
            for (int i = 0; i < levelsProp.arraySize; i++)
            {
                SerializedProperty level = levelsProp.GetArrayElementAtIndex(i);
                float yVal = level.FindPropertyRelative("yStart").floatValue;
                Color col = level.FindPropertyRelative("color").colorValue;

                // Переводим значение 0..1 в пиксели внутри Rect'а кривой
                // В Unity UI Y=0 — это низ, но в GUI Y=0 — это верх, поэтому инвертируем (1 - yVal)
                float pixelY = curveRect.y + (curveRect.height * (1f - yVal));

                // Рисуем горизонтальную линию
                Handles.BeginGUI();
                Handles.color = col;
                Handles.DrawLine(
                    new Vector3(curveRect.x, pixelY), 
                    new Vector3(curveRect.x + curveRect.width, pixelY), 
                    2f // Толщина линии
                );
                Handles.EndGUI();

                // Подпись сложности
                GUIStyle style = new GUIStyle(EditorStyles.miniLabel) { normal = { textColor = col } };
                GUI.Label(new Rect(curveRect.x + 5, pixelY - 15, 100, 15), level.FindPropertyRelative("name").stringValue, style);
            }
        }

        EditorGUI.EndProperty();
    }

    // Указываем высоту элемента, чтобы он не перекрывал следующие поля
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 105f; 
    }
}