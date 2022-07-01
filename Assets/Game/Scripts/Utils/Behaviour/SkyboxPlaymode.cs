using UnityEngine;

namespace Game.Scripts.Utils.Behaviour
{
    public class SkyboxPlaymode : MonoBehaviour
    {
        private void Start()
        {
            Skybox skybox = GetComponent<Skybox>();
            skybox.material = Instantiate(skybox.material);
        }
    }
}