using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mechanics
{
    public class GamaManager : MonoBehaviour
    {
        private Player _player;

        private void Start()
        {
            _player = FindObjectOfType<Player>();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void EndGame()
        {
            _player.enabled = false;
            Invoke(nameof(RestartGame), 0.5f);
        }

        private void RestartGame()
        {
            DefaultSettings();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void DefaultSettings()
        {
            
        }
    }
}
