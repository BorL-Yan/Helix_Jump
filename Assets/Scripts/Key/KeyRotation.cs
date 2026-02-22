using System;
using UnityEngine;

namespace Key
{
    public class KeyRotation : MonoBehaviour
    {
        [SerializeField] private float _speedRotation;


        private void Update()
        {
            Vector3 curentrotation = transform.eulerAngles;
            curentrotation.y += _speedRotation * Time.deltaTime;
            transform.rotation = Quaternion.Euler(curentrotation);
        }
        
    }
}