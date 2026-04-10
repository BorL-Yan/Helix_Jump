using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static Action<Camera> OnCameraChanged;
    
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private Camera _cutsceneCamera;

    private Camera _currentActiveCamera;

    public void SwitchCamera(Camera newCamera)
    {
        if (_currentActiveCamera == newCamera) return;
        
        // Выключаем текущую, включаем новую
        if (_currentActiveCamera != null) _currentActiveCamera.enabled = false;
        
        //_currentActiveCamera = newCamera;
        
        OnCameraChanged?.Invoke(newCamera);
    }
}