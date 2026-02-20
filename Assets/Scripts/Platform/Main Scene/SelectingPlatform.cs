using System;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;

namespace Platform.Main_Scene
{
    public class SelectingPlatform : MonoBehaviour
    {
        [SerializeField] private MainPlatform _mainPlatform;
        
        private void OnEnable()
        {
            GameManager.Instance.Action.SelectPlatform += GameLifeTime;
            GameManager.Instance.Action.ActivateGameSelectPlatform += ActivateGame;
        }

        private void Start()
        {
            int level = GameSave.GetSettings().Level; 
            if (level > _mainPlatform.platformID)
            {
                Activate();
            }
            else if (level < _mainPlatform.platformID)
            {
                Deactivate();
            }
        }
        
        private void SelectPlatform(int id, Action<SelectingPlatform> callback)
        {
            if (_mainPlatform.platformID == id)
            {
                _mainPlatform.Select();
                callback?.Invoke(this);
            }else if (id > _mainPlatform.platformID)
            {
                Activate();
            }
            else if (id < _mainPlatform.platformID)
            {
                Deactivate();
            }
        }
        
        private void GameLifeTime(int id)
        {
            if (GameManager.Instance.OpenNewLevel && _mainPlatform.platformID == id)
            {
                Sequence sequence = DOTween.Sequence();
                sequence.AppendInterval(2f) // Animation interval
                    .AppendCallback(() =>
                    {
                        _mainPlatform.Select();
                        GameManager.Instance.Action.SelectingPlatform?.Invoke(this);
                        GameManager.Instance.OpenNewLevel = false;
                    });
            }
            else
            {
                SelectPlatform(id, GameManager.Instance.Action.SelectingPlatform);
            }


        }

        private void ActivateGame(int id)
        {
            SelectPlatform(id, GameManager.Instance.Action.ActivateGameSelectingPlatform);
            
        }
        

        public void Select()
        {
            _mainPlatform.Select();
        }
        public void Activate()
        {
            _mainPlatform.Activate();
        }

        public void Deactivate()
        {
            _mainPlatform.Deactivate();
        }

        public void PlatformJumping()
        {
            _mainPlatform.JumpEffect();
        }
        
        
        private void OnValidate()
        {
            if (_mainPlatform == null)
            {
                _mainPlatform = GetComponentInParent<MainPlatform>();
            }
        }
        
        [ProButton]
        private void Reset()
        {
            _mainPlatform = GetComponentInParent<MainPlatform>();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Action.SelectPlatform -= GameLifeTime;
                GameManager.Instance.Action.ActivateGameSelectPlatform -= ActivateGame;
            }
        }
    }
}