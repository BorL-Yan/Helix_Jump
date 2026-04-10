using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI_Scripts.Skin
{
    public class BuySkin : UIButton
    {
        [SerializeField] private TMP_Text _price;
        [SerializeField] private SkinManager _skinManager;
        [SerializeField] private GameObject _closeIcon;
        private int currentPrice;
        private bool activateBuy;

        [SerializeField] private GameObject[] _gameObjects;
        [SerializeField] private CanvasGroup _canvasGroup;
        
        private Image _raycastImage;
        
        private void Awake()
        {
            _raycastImage = GetComponent<Image>();
        }

        private void OnEnable()
        {
            activateBuy = true;
            _closeIcon.SetActive(false);

        }

        public void UpdatePrice()
        {
            currentPrice = 100 * ((_skinManager.GetActiveSkinCount() - 1) * 6);
            currentPrice = Mathf.Max(100, currentPrice);
            
            _price.text = currentPrice.ToString();
            
            
        }
        
        protected override void Click()
        {
            Buy();
        }

        private void Buy()
        {
            var settings = GameSave.GetSettings();
            
            if(settings.Coin < currentPrice || !activateBuy) return;
            activateBuy = false;
            _raycastImage.enabled = false;
            
            _skinManager.SelectAnimation(() =>
            {
                activateBuy = true;
                UpdatePrice();
                _closeIcon.SetActive(false);
                _raycastImage.enabled = true;
                foreach (var obj in _gameObjects)
                {
                    obj.transform.localScale = Vector3.zero;
                    obj.transform.DOScale(1, 0.5f);
                }

                _canvasGroup.alpha = 0;
                _canvasGroup.DOFade(1, 0.5f);
            });
            _closeIcon.SetActive(true);

            settings.Coin -= currentPrice;
            GameSave.SetSettings(settings);
            GameSave.Save();
        }
        
    }
}