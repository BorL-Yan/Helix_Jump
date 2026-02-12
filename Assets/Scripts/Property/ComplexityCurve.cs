using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class ComplexityLevel
{
    public string name;
    [Range(0f, 1f)] public float yStart; // Где на оси Y начинается эта сложность
    [HideInInspector] public Color color; // Цвет сгенерируем один раз

    public ComplexityLevel(string name, float yStart)
    {
        this.name = name;
        this.yStart = yStart;
        // Генерируем случайный приятный глазу цвет
        this.color = UnityEngine.Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.6f, 0.9f);
    }
}

[Serializable]
public class ComplexityCurve
{
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    public List<ComplexityLevel> levels = new List<ComplexityLevel>();
}