using System;
using TMPro;
using UnityEngine;

namespace UI_Scripts
{
    public class CoinText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coin;
        
        private void UpdateUI()
        {
            _coin.text = GameSave.GetSettings().Coin.ToString();
        }

        private void OnEnable()
        {
            GameSave.OnChangeSettings += UpdateUI;
            UpdateUI();
        }

        private void OnDisable()
        {
            GameSave.OnChangeSettings -= UpdateUI;
        }
    }
}