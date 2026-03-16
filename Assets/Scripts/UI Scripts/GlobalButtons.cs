using System;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace UI_Scripts
{
    public class GlobalButtons : MonoBehaviour
    {
        private GameAction _gameAction;

        [SerializeField] private Transform _panel;
        [SerializeField] private CanvasGroup _group;
        
        
        
        [Inject]
        public void Construct(GameAction gameAction)
        {
            _gameAction = gameAction;
        }

        private void Awake()
        {
            Activate(true);
            _gameAction.ActivateGlobalPanel += Activate;
        }

        private void OnDestroy()
        {
            if(_gameAction != null)
                _gameAction.ActivateGlobalPanel -= Activate;
        }

        private void Activate(bool value)
        {
            if (value)
            {
                _group.alpha = 1;
                _panel.gameObject.SetActive(true);
            }
            else
            {
                _group.DOFade(0, 0.3f).OnComplete(() =>
                { 
                    _panel.gameObject.SetActive(false);
                });
            }
        } 
    }
}