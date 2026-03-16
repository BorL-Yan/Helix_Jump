using System.Collections.Generic;
using com.cyborgAssets.inspectorButtonPro;
using DG.Tweening;
using UnityEngine;

namespace Platform.Multy
{
    public class MultyPlatformController : MonoBehaviour
    {
        [SerializeField] private List<MultyPlatformActivateList> _activateList;
        
        [VInspector.Button]
        public void Activate()
        {
            Sequence seq = DOTween.Sequence();
            for (int i = 0; i < _activateList.Count; i++)
            {
                var i1 = i;
                seq.AppendCallback(() =>
                {
                    _activateList[i1].Activate();
                })
                .AppendInterval(0.1f);
            }
        }
        [VInspector.Button]
        public void Deactivate()
        {
            foreach (var platform in _activateList)
                platform.Deactivate();
        }

        private void Start() => Deactivate();
        
        [VInspector.Button]
        private void Reset()
        {
            UpdatePlatformList();
        }

        private void UpdatePlatformList()
        {
            var children = GetComponentsInChildren<MultyPlatformActivateList>(true);
            _activateList = new List<MultyPlatformActivateList>(children);
        }
    }
}