using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mechanics
{
    public class GamaManager : MonoBehaviour
    {
        // ReSharper disable Unity.PerformanceAnalysis
        public void EndGame()
        {
            Invoke(nameof(RestartGame), 0.5f);
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
