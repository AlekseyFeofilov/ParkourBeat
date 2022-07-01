using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Settings.AudioSettings
{
    public class AudioSettings : MonoBehaviour
    {
        public static float TotalVolume = 1;
        public static float RealMusicVolume = 1;
        public static float RealSoundVolume = 1;
        
        public static float MusicVolume => TotalVolume * RealMusicVolume;
        public static float SoundVolume => TotalVolume * RealSoundVolume;

        [SerializeField] private Slider totalVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundVolumeSlider;
        [SerializeField] private List<AudioSource> musicSources;
        [SerializeField] private List<AudioSource> soundSources;
        
        private void Awake()
        {
            totalVolumeSlider.value = TotalVolume;
            musicVolumeSlider.value = RealMusicVolume;
            soundVolumeSlider.value = RealSoundVolume;
        }

        public void ChangeTotalVolume(float volume)
        {
            TotalVolume = volume;
            foreach (var audioSource in musicSources)
            {
                audioSource.volume = MusicVolume;
            }
            foreach (var audioSource in soundSources)
            {
                audioSource.volume = SoundVolume;
            }
        }
        
        public void ChangeMusicVolume(float volume)
        {
            RealMusicVolume = volume;
            foreach (var audioSource in musicSources)
            {
                audioSource.volume = MusicVolume;
            }
        }
        
        public void ChangeSoundVolume(float volume)
        {
            RealSoundVolume = volume;
            foreach (var audioSource in soundSources)
            {
                audioSource.volume = SoundVolume;
            }
        }
    }
}