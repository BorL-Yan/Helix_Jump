using UnityEngine;

namespace Ball.Controller
{
    [RequireComponent(typeof(AudioSource))]
    public class BallBreakPlatformSound : MonoBehaviour
    {
        private AudioSource _audioSource;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayBreakPlatform(int count = 1)
        {
            count = count <= 0 ? 1 : count;
            float smoothness = 7;
            float value = Mathf.Lerp(3f, 1, 1 / (1  + (count -1) / smoothness));

            AudioClip clip = SoundManager.Instance.AudioClips[SoundType.Platform_Break];
            _audioSource.pitch = value;
            _audioSource.PlayOneShot(clip);
        }
    }
}