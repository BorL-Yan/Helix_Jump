using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformDifficulty { Normal, Medium, Hard }

[CreateAssetMenu(fileName = "NewLevelProfile", menuName = "Helix/Level Profile")]
public class LevelProfile : ScriptableObject
{
    [Header("Общие настройки")]
    public int totalPlatforms = 50;
    public float distanceBetweenPlatforms = 2.0f;
    
    [Header("Сложность")]
    // Кривая позволяет менять сложность от начала (0.0) к концу уровня (1.0)
    public PlatformDifficultyCurve difficultyCurve;
    [MinMaxRange(0f,1f)] public Vector2 probility; 
    
    
    [Header("Вращение")]
    [Tooltip("Минимальный и максимальный поворот следующей платформы относительно предыдущей")]
    [MinMaxRange(0f,360f)]
    public Vector2 rotationStepRange = new Vector2(30f, 150f);
    public AnimationCurve rotationStepCurve = AnimationCurve.Linear(0, 0, 1, 1);
    
    [Header("Коллекция платформ")]
    public List<GameObject> normalPlatforms; 
    public List<GameObject> mediumPlatforms;
    public List<GameObject> hardPlatforms;
   
    public List<GameObject> GetPlatformList(PlatformDifficulty difficulty)
    {
        switch (difficulty)
        {
            case PlatformDifficulty.Normal: return normalPlatforms;
            case PlatformDifficulty.Medium: return mediumPlatforms;
            case PlatformDifficulty.Hard: return hardPlatforms;
            default: return normalPlatforms;
        }
    }
}

[Serializable]
public class PlatformDifficultyCurve
{
    // Сама кривая
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

    // Пороги сложности (Y-координаты)
    [Range(0f, 1f)] public float mediumThreshold = 0.4f;
    [Range(0f, 1f)] public float hardThreshold = 0.8f;

    // Цвета для линий (храним здесь, чтобы не менялись при каждом кадре)
    [HideInInspector] public Color mediumColor = Color.yellow;
    [HideInInspector] public Color hardColor = Color.red;

    // Метод получения сложности по значению Y
    public PlatformDifficulty GetDifficultyAt(float xTime)
    {
        float yValue = curve.Evaluate(xTime);
        if (yValue >= hardThreshold) return PlatformDifficulty.Hard;
        if (yValue >= mediumThreshold) return PlatformDifficulty.Medium;
        return PlatformDifficulty.Normal;
    }
}


