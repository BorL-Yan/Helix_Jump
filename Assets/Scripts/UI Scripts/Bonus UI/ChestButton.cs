using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace UI_Scripts.Bonus_UI
{
    public class ChestButton : MonoBehaviour
    {
        [SerializeField] private List<BestPrizeIcon> _bestPrizes;
        private UIButton _uiButton;

        [SerializeField] private Animator _chestAnim;
        private BestPrize _bestPrize;
        [SerializeField] private ParticleSystem _startParticle; 

        private void Awake()
        {
            _uiButton = GetComponent<UIButton>();
            _startParticle.gameObject.SetActive(false);
            _bestPrizes.ForEach(item => item.Icon.SetActive(false));
        }

        #region Chest Anim

        public void OpenChest()
        {
            SoundManager.Instance.Play(SoundType.Open_Mini_Chest);
            PlayAnim("Open");
        }

        public void RotateChest() => PlayAnim("Idle");
        public void SpawnChest() => PlayAnim("Spawn");

        private void PlayAnim(string aName) => _chestAnim.Play(aName, 0, 0);
        
        #endregion
        
        
        private void Click()
        {
            _bestPrize.OpenChest(value =>
            {
                if (value != BestPrizeType.Null)
                {
                    Open(value);
                }
            }, this);
        }
        
        
        public void SetBestPrizeComponent(BestPrize bp) => _bestPrize = bp;
        
        private void Open(BestPrizeType prizeType)
        {
            _uiButton.enabled = false;
            GameObject icon = null;
            foreach (var prize in _bestPrizes)
            {
                if (prize.PrizeType == prizeType)
                {
                    icon = prize.Icon;
                    prize.Icon.SetActive(true);
                }
                else
                {
                    prize.Icon.SetActive(false);
                }
            }

            if (icon != null)
            {
                Sequence sequence = DOTween.Sequence();
                icon.transform.localScale = Vector3.one * 0.5f;
                sequence.Append(icon.transform.DOScale(1, 0.7f));
                _startParticle.gameObject.SetActive(true);
            }
            
        }

        private void OnEnable()
        {
            _uiButton.OnClick += Click;
        }

        private void OnDisable()
        {
            _uiButton.OnClick -= Click;
        }
    }

    [System.Serializable]
    public struct BestPrizeIcon
    {
        public BestPrizeType PrizeType;
        public GameObject Icon;
    }
}