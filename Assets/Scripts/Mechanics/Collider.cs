using UnityEngine;

namespace Mechanics
{
    public class Collider : MonoBehaviour
    {
        public Player player;

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
