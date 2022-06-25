using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class TransitionToAnotherScene : MonoBehaviour
    {
        void TransitionToMenuMap()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}