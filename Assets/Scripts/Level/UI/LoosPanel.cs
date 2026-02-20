using UnityEngine;

namespace Level
{
    public class LoosPanel : MonoBehaviour
    {
        [SerializeField] private GameObject loosPanel;

        private void Start()
        {
            SetActive(false);
        }
        
        public void SetActive(bool value)
        {
            loosPanel.SetActive(value);
        }
    }
}