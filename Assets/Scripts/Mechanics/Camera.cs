using UnityEngine;

namespace Mechanics
{
    public class Camera : MonoBehaviour
    {
        private Player _player;
        private Vector3 _offset;

        void Start()
        {
            _player = FindObjectOfType<Player>();
            _offset = transform.position;
        }

        void Update()
        {
            transform.position = _player.transform.position + _offset;
        }
    }
}
