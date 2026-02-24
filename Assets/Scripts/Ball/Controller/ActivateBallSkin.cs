using System.Collections.Generic;
using UnityEngine;

namespace Ball.Controller
{
    public class ActivateBallSkin : MonoBehaviour
    {
        [SerializeField] private List<BallSkin> _ballSkins;

        private GameObject _activeSkin;
        
        private void Awake()
        {
            foreach (var item in _ballSkins)
            {
                item.skin.SetActive(false);
            }
            
            var ballskin = GameSave.GetSettings().ActiveBallSkin;
            _activeSkin = _ballSkins.Find(x => x.skinType == ballskin).skin;
            _activeSkin.SetActive(true);
        }

        
        private void OnEnable()
        {
            GameManager.Instance.Action.OnSelectBallSkin += Activate;
        }

        private void OnDisable()
        {
            GameManager.Instance.Action.OnSelectBallSkin -= Activate;
        }

        private void Activate(BallSkinType skinType)
        {
            var skin = _ballSkins.Find(x => x.skinType == skinType);
            _activeSkin.SetActive(false);
            _activeSkin = skin.skin;
            _activeSkin.SetActive(true);
        }
        
    }
}