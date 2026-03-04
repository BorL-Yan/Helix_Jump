using System.Collections.Generic;
using UnityEngine;

namespace Ball.Controller
{
    public class ActivateBallSkin : MonoBehaviour
    {
        [SerializeField] private List<BallSkin> _ballSkins;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private GameObject _activeSkin;
        
        private void Awake()
        {
            SelectBallSkin();
            _spriteRenderer = GetComponent<ChangeBallColor>()._spriteRenderer;
        }

        private void SelectBallSkin()
        {
            foreach (var item in _ballSkins)
            {
                item.skin.SetActive(false);
            }
            
            var ballskin = GameSave.GetSettings().ActiveBallSkin;
            
            var skin =_ballSkins.Find(x => x.skinType == ballskin);
            _activeSkin = skin.skin;
            _activeSkin.SetActive(true);
            _spriteRenderer.sprite = skin.sprite;
        }
        
        
        private void OnEnable()
        {
            SelectBallSkin();
            GameManager.Instance.Action.OnSelectBallSkin += Activate;
        }

        private void OnDisable()
        {
            if(GameManager.Instance != null)
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