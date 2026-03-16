using UnityEngine;

namespace Platform.Multy
{
    public class MultyPlatformDetect : MonoBehaviour
    {
        public MultyPlatformActivateList ActivateList { get; private set; }

        private void Awake()
        {
            ActivateList = GetComponentInParent<MultyPlatformActivateList>();
        }
        
        public void Activate()
        {
            Debug.Log("Activate text Effect");
            ActivateList.ActivateDetect();
        }
    }
}