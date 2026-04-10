using System;
using UnityEngine;

namespace Level
{
    public class ChampionLeagueUI : MonoBehaviour
    {
        private Action _callBack;

        public void Activate(Action callback)
        {
            _callBack = callback;
            gameObject.SetActive(true);
        }
        
        
        
        public void EndAnimation()
        {
            _callBack?.Invoke();
            gameObject.SetActive(false);
        }
    }
}