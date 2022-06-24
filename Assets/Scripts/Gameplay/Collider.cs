using UnityEngine;

namespace Gameplay
{
    public class Collider : MonoBehaviour
    {
        [SerializeField] private PlayerMovement player;

        private void OnTriggerEnter(UnityEngine.Collider other)
        {
            player.CollisionEnter(tag);
        }

        private void OnTriggerExit(UnityEngine.Collider other)
        {
            player.CollisionExit(tag);
        }
    }
}
