using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.UI
{
    public class SceneLoader : MonoBehaviour
    {
        public void SceneLoad(int index)
        {
            SceneManager.LoadScene(index);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}