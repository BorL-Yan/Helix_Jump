using System;
using Unity.Content;
using UnityEngine;

namespace UIScript
{
    public class SetCaonvasTargetCamera : MonoBehaviour
    {
        private Canvas _canvas;
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }
        private void Start()
        {
            // _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            //
            Camera mainCam = Camera.main;
            // if (mainCam != null)
            // {
            //     _canvas.worldCamera = mainCam;
            // }
            // else
            // {
            //     Debug.LogError("Не найдена камера с тегом 'MainCamera' или камера выключена!");
            // }

            UpdateCanvasCamera(mainCam);
        }
        
        private void OnEnable()
        {
            CameraManager.OnCameraChanged += UpdateCanvasCamera;
        }

        private void OnDisable()
        {
            CameraManager.OnCameraChanged -= UpdateCanvasCamera;
        }

        private void UpdateCanvasCamera(Camera newCamera)
        {
            if (_canvas == null) return;
            
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = newCamera;
            Debug.Log("SetCamera");
        }
        
    }
}