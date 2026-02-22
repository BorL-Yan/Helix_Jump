using System;
using UnityEngine;

namespace Level
{
    public class BestPrize : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;

        [SerializeField] private UIButton _closeButton;
        
        // TODO Box List;
        
        private Action _callback;
        
        private void Start()
        {
            _panel.SetActive(false);
            _closeButton.gameObject.SetActive(false);
        }

        public void Activate(Action callback)
        {
            _callback = callback;
            _panel.SetActive(true);
            //TODO change this activate to opening all boxes cont.  
            ActivateCloseButton();
        }

        public void ActivateCloseButton()
        {
            _closeButton.gameObject.SetActive(true);
        }
        

        private void Deactivate()
        {
            _callback?.Invoke();
            _panel.SetActive(false);
        }
        
        private void OnEnable()
        {
            _closeButton.OnClick += Deactivate;
        }

        private void OnDisable()
        {
            _closeButton.OnClick -= Deactivate;
        }
    }
}