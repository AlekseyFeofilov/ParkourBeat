using Game.Scripts.Scenes;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class SceneLoader : MonoBehaviour
    {
        public void SceneLoad(int index)
        {
            AdvancedSceneManager.LoadScene(index);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}