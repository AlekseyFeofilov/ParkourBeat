using UnityEngine;

namespace Game.Scripts.Settings.AudioSettings
{
    public class MusicAudioSource : MonoBehaviour
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.volume = Settings.AudioSettings.AudioSettings.MusicVolume;
        }
    }
}