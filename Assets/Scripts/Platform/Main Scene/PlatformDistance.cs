using System;
using UnityEngine;

namespace Platform.Main_Scene
{
    public class PlatformDistance : MonoBehaviour
    {
        [SerializeField] private float _ditanceX;
        [SerializeField] private float _ditanceY;



#if UNITY_EDITOR
        [VInspector.Button]
        private void OnValidate()
        {
            var childes = GetComponentsInChildren<MainPlatform>();
            for (int i = 0; i < childes.Length; i++)
            {
                var item = childes[i];
                
                float ditanceX = _ditanceX * i;
                float distanceY = _ditanceY * (i % 2 != 0 ? -1 : 1);
                item.gameObject.transform.position = new Vector3(distanceY, 0, ditanceX);
                item.platformID = i+1;
            }
        }
#endif        
    }
}