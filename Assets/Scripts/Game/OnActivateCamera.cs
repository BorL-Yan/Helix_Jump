using System;
using UnityEngine;

namespace UIScript.Game
{
    public class OnActivateCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera; 
        private void OnEnable()
        {
            CameraManager.OnCameraChanged?.Invoke(_camera);
        }
    }
}