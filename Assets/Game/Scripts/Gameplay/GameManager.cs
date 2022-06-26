using Game.Scripts.Gameplay.Player;
using Game.Scripts.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovement player;

        // ReSharper disable Unity.PerformanceAnalysis
        public void EndGame(float timeDelay)
        {
            player.enabled = false;
            Invoke(nameof(RestartGame), timeDelay);
        }

        private void RestartGame()
        {
            DefaultSettings();
            AdvancedSceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void DefaultSettings()
        {
            
        }
    }
}
