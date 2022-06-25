using UnityEngine;

namespace Game.Scripts.Gameplay.Player
{
    public class PlayerCollider : MonoBehaviour
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
