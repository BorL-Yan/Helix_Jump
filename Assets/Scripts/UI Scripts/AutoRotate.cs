using System;
using UnityEngine;

namespace UI_Scripts
{
    public class AutoRotate : MonoBehaviour
    {
        private void Update()
        {
            Vector3 current = transform.eulerAngles;
            current.z += 20 * Time.deltaTime;
            transform.eulerAngles = current;
        }
    }
}