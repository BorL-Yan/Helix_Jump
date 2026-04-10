using UnityEngine;

namespace Key
{
    public class BoxAnimationEvent : MonoBehaviour
    {
        [SerializeField] private OpenKeys _openKeys;

        public void BoxOpen()
        {
            _openKeys.BoxOpened();
            SoundManager.Instance.Play(SoundType.Open_Big_Chest);
        }
    }
}