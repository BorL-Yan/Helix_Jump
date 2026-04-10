using System;
using UnityEngine;

namespace Level
{
    public class RankUpController : MonoBehaviour
    {
        public UIButton _button;

        private Action _callback;

        public void PlaySound(SoundType type) => SoundManager.Instance.Play(type);
        
        
        public void Activate(Action callback)
        {
            gameObject.SetActive(true);
            _callback = callback;
        }
        
        private void EndAnim()
        {
            gameObject.SetActive(false);
            _callback?.Invoke();
        }

        public void OnEnable()
        {
            _button.OnClick += EndAnim;
        }

        private void OnDisable()
        {
            _button.OnClick -= EndAnim;
        }
    }
}