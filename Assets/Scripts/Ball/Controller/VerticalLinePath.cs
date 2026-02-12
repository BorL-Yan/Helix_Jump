using UnityEngine;
using System.Collections.Generic;

public class VerticalLinePath : MonoBehaviour
{
    public float trailLifetime = 0.5f; // Время жизни хвоста в секундах
    public float minDistance = 0.1f;   // Дистанция между новыми точками
    
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private List<float> spawnTimes = new List<float>();

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        
        // Настройка градиента (можно сделать и в инспекторе)
        // Важно: в градиенте конец должен уходить в Alpha 0
    }

    void Update()
    {
        ClearOldPoints();
        AddNewPoint();
        UpdateLineRenderer();
    }

    void AddNewPoint()
    {
        // Добавляем точку, только если шарик сдвинулся
        if (points.Count == 0 || Vector3.Distance(transform.position, points[points.Count - 1]) > minDistance)
        {
            // Здесь можно зафиксировать ось, если нужно (например, только X и Y)
            points.Add(transform.position);
            spawnTimes.Add(Time.time);
        }
    }

    void ClearOldPoints()
    {
        // Удаляем точки, которые "протухли" по времени
        while (spawnTimes.Count > 0 && Time.time - spawnTimes[0] > trailLifetime)
        {
            spawnTimes.RemoveAt(0);
            points.RemoveAt(0);
        }
    }

    void UpdateLineRenderer()
    {
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}