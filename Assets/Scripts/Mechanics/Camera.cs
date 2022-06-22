using UnityEngine;

namespace Mechanics
{
    public class Camera : MonoBehaviour
    {
        private Player _player;
        private Vector3 _offset;

        private void Start()
        {
            _player = FindObjectOfType<Player>();
            _offset = transform.position;
        }

        private void Update()
        {
            var playerPosition = _player.transform.position;
            var cameraTransform = transform;
            
            cameraTransform.position = new Vector3(
                playerPosition.x + _offset.x,
                cameraTransform.position.y,
                playerPosition.z + _offset.z
            );
        }
    }
}