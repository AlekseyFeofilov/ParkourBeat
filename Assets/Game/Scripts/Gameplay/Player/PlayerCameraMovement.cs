using UnityEngine;

namespace Game.Scripts.Gameplay.Player
{
    public class PlayerCameraMovement : MonoBehaviour
    {
        [SerializeField] private float rotateSpeedH = 250f;
        [SerializeField] private float rotateSpeedV = 250f;
        [SerializeField] private float minYaw = 45;
        [SerializeField] private float maxYaw = 135;
        [SerializeField] private float minPitch;
        [SerializeField] private float maxPitch = 45;
        
        private Vector3 _offset;
        private float _yaw;
        private float _pitch;
        
        private void Start()
        {
            var eulerAngles = transform.eulerAngles;
            _pitch = eulerAngles.x;
            _yaw = eulerAngles.y;
        }

        private void Update()
        {
            _yaw += rotateSpeedH * Input.GetAxis("Mouse X") * Time.deltaTime;
            _pitch -= rotateSpeedV * Input.GetAxis("Mouse Y") * Time.deltaTime;

            if (_yaw < minYaw) _yaw = minYaw;
            if (_yaw > maxYaw) _yaw = maxYaw;
            if (_pitch < minPitch) _pitch = minPitch;
            if (_pitch > maxPitch) _pitch = maxPitch;

            transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);
        }
    }
}