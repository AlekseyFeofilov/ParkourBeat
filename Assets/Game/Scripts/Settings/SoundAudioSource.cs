using UnityEngine;

namespace Game.Scripts.Settings
{
    public class SoundAudioSource : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = AudioSettings.SoundVolume;
        }
    }
}