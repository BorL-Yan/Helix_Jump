using System;
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
            currentPrice = 100 * ((_skinManager.GetActiveSkinCount() - 1) * 4);
            currentPrice = Mathf.Max(100, currentPrice);
            
            Debug.Log($"PRice : {currentPrice}");
            
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
            });
            _closeIcon.SetActive(true);

            settings.Coin -= currentPrice;
            GameSave.SetSettings(settings);
            GameSave.Save();
        }
    }
}