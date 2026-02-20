using UnityEngine;

namespace UI_Scripts
{
    public class PlayButtonController : MonoBehaviour
    {
        [SerializeField] private GameObject _playButton;
        
        public void SetActive(bool active)
        {
            _playButton.SetActive(active);
        }
    }
}