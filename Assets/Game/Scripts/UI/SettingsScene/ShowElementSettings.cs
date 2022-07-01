using UnityEngine;

namespace Game.Scripts.UI.SettingsScene
{
    public class ShowElementSettings : MonoBehaviour
    {
        [Header("Components")] [SerializeField]
        private GameObject _soundPref;

        [SerializeField] private GameObject _graphicsPref;
        [SerializeField] private GameObject _controlPref;

        public void ShowSound()
        {
            _soundPref.gameObject.SetActive(true);
            _graphicsPref.gameObject.SetActive(false);
            _controlPref.gameObject.SetActive(false);
        }

        public void ShowGraphics()
        {
            _soundPref.gameObject.SetActive(false);
            _graphicsPref.gameObject.SetActive(true);
            _controlPref.gameObject.SetActive(false);
        }

        public void ShowControl()
        {
            _soundPref.gameObject.SetActive(false);
            _graphicsPref.gameObject.SetActive(false);
            _controlPref.gameObject.SetActive(true);
        }
    }
}