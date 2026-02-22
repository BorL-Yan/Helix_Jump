using System;
using UnityEngine;

namespace Test
{
    public class DrawCube : MonoBehaviour
    {
        public Vector3 size;


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}