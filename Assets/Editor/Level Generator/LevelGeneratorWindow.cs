using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class LevelGeneratorWindow : EditorWindow
{
    private LevelProfile levelProfile;
    private Transform parentContainer;
    
    private GameObject startPlatform; 
    private GameObject finishPlatform;

    [MenuItem("Tools/Helix Level Generator")]
    public static void ShowWindow()
    {
        GetWindow<LevelGeneratorWindow>("Helix Gen");
    }

    private void OnEnable()
    {
        // Вызываем загрузку при открытии окна или пересборке проекта
        LoadResources();
    }

    private void LoadResources()
    {
        // Предположим, префабы лежат прямо в Assets/Resources/
        // Если лежат в подпапке, например Resources/Platforms, то путь будет "Platforms/Start"
        startPlatform = Resources.Load<GameObject>("Platforms/Start");
        finishPlatform = Resources.Load<GameObject>("Platforms/Finish");

        // Проверка на ошибки
        if (startPlatform == null) Debug.LogWarning("Start Platform не найдена в Resources!");
        if (finishPlatform == null) Debug.LogWarning("Finish Platform не найдена в Resources!");
    }
    
    private void OnGUI()
    {
        GUILayout.Label("Генератор из пула префабов", EditorStyles.boldLabel);

        levelProfile = (LevelProfile)EditorGUILayout.ObjectField("Profile", levelProfile, typeof(LevelProfile), false);
        parentContainer = (Transform)EditorGUILayout.ObjectField("Parent Container", parentContainer, typeof(Transform), true);
        
        GUILayout.Space(10);
        GUILayout.Label("Особые платформы (Опционально)", EditorStyles.miniLabel);
        startPlatform = (GameObject)EditorGUILayout.ObjectField("Start Platform", startPlatform, typeof(GameObject), false);
        finishPlatform = (GameObject)EditorGUILayout.ObjectField("Finish Platform", finishPlatform, typeof(GameObject), false);

        GUILayout.Space(20);

        if (GUILayout.Button("Сгенерировать уровень"))
        {
            if (levelProfile != null && parentContainer != null)
            {
                GenerateLevel();
            }
            else
            {
                Debug.LogError("Не назначен Профиль или Родительский контейнер!");
            }
        }
        
        if (GUILayout.Button("Очистить"))
        {
            ClearLevel();
        }
    }

    private void GenerateLevel()
    {
        ClearLevel(); // Сначала чистим старое

        float currentY = 0;
        float currentRotation = 0;

        for (int i = 0; i < levelProfile.totalPlatforms; i++)
        {
            GameObject prefabToSpawn = null;
            bool isFinish = (i == levelProfile.totalPlatforms - 1);

            // 1. Логика выбора префаба
            if (i == 0 && startPlatform != null)
            {
                // Самая первая платформа
                prefabToSpawn = startPlatform;
            }
            else if (isFinish && finishPlatform != null)
            {
                // Финишная платформа
                prefabToSpawn = finishPlatform;
            }
            else
            {
                // Обычная генерация на основе сложности
                float progress = (float) i / levelProfile.totalPlatforms;
                
                
                float curveValue = levelProfile.difficultyCurve.curve.Evaluate(progress);
                
                curveValue = Mathf.Lerp(0, curveValue, 
                    Random.Range(levelProfile.probility.x, levelProfile.probility.y));
                
                PlatformDifficulty difficulty = GetDifficultyFromCurve(curveValue);
                
                List<GameObject> pool = levelProfile.GetPlatformList(difficulty);

                if (pool != null && pool.Count > 0)
                {
                    prefabToSpawn = pool[Random.Range(0, pool.Count)];
                }
                
               
            }

            // Если список пуст или префаб не найден, пропускаем итерацию (или ставим заглушку)
            if (prefabToSpawn == null) continue;

            // 2. Спавн
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabToSpawn, parentContainer);
            instance.transform.localPosition = new Vector3(0, currentY, 0);
            
            // 3. Вращение
            // Фишка Helix Jump: дырка не должна совпадать с предыдущей.
            // Мы берем предыдущий поворот и добавляем случайное смещение.
            
            // Если это не первая платформа, крутим её
            if (i > 0)
            {
                float progress = (float) i / levelProfile.totalPlatforms;
                float rotationCurveValue = levelProfile.rotationStepCurve.Evaluate(progress);
                
                float rotationStep = Random.Range(levelProfile.rotationStepRange.x, levelProfile.rotationStepRange.y)
                                     * rotationCurveValue;
                // 50% шанс повернуть влево или вправо
                if (Random.value > 0.5f) rotationStep *= -1;
                
                currentRotation += rotationStep;
            }

            instance.transform.localRotation = Quaternion.Euler(0, currentRotation, 0);

            // Для удобства в иерархии
            instance.name = $"[{i}] {prefabToSpawn.name}";

            Debug.Log($"[{i}] {prefabToSpawn.name}");
            
            // Регистрация для Undo (Ctrl+Z)
            Undo.RegisterCreatedObjectUndo(instance, "Spawn Platform");

            // 4. Смещаем высоту
            currentY -= levelProfile.distanceBetweenPlatforms;
        }
        
        var effect = Resources.Load<GameObject>("Partical/Finish");
        var instantiatePrefab = (GameObject)PrefabUtility.InstantiatePrefab(effect, parentContainer);
        
    }

    private void ClearLevel()
    {
        if (parentContainer == null) return;
        while (parentContainer.childCount > 0)
        {
            Undo.DestroyObjectImmediate(parentContainer.GetChild(0).gameObject);
        }
    }

    private PlatformDifficulty GetDifficultyFromCurve(float value)
    {
        if (levelProfile.difficultyCurve.mediumThreshold > value) return PlatformDifficulty.Normal;
        if (levelProfile.difficultyCurve.hardThreshold > value) return PlatformDifficulty.Medium;
        return PlatformDifficulty.Hard;
    }
}