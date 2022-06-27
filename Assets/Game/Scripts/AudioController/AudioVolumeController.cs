using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    //оставила для Ромы
    public class AudioVolumeController : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private Slider totalAudio;

        [SerializeField] private Slider musicAudio;
        [SerializeField] private Slider soundsAudio;

        private static float _totalVolume;
        private static float _musicVolume;
        private static float _soundsVolume;

        private void Update()
        {
            _totalVolume = totalAudio.value;
            _musicVolume = musicAudio.value;
            _soundsVolume = soundsAudio.value;
        }
    }
}