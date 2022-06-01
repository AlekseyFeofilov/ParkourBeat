using UnityEngine;

namespace MapEditor
{
    public class Camera : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public float shiftMultiplier = 2f;

        public float lookSpeedH = 2.5f;
        public float lookSpeedV = 2.5f;
        public float zoomSpeed = 2.5f;
        public float dragSpeed = 50f;

        private float _yaw;
        private float _pitch;

        private void Start()
        {
            // Initialize the correct initial rotation
            var eulerAngles = transform.eulerAngles;

            _yaw = eulerAngles.y;
            _pitch = eulerAngles.x;
        }

        private void Update()
        {
            //Zoom and Move with Keyboard
            var speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? shiftMultiplier : 1f) * Time.deltaTime;
            var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            move *= speed;
            
            transform.Translate(move);

            //drag camera around with Middle Mouse
            if (Input.GetMouseButton(2))
            {
                transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * dragSpeed,
                    -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * dragSpeed, 0);
            }

            //Zoom in and out with Mouse Wheel
            transform.Translate(
                0,
                0,
                Input.GetAxis("Mouse ScrollWheel") * zoomSpeed *
                (Input.GetKey(KeyCode.LeftShift) ? shiftMultiplier : 1f),
                Space.Self);

            if (!Input.GetKey(KeyCode.LeftAlt))
            {
                //Look around with Right Mouse
                if (Input.GetMouseButton(1))
                {
                    _yaw += lookSpeedH * Input.GetAxis("Mouse X");
                    _pitch -= lookSpeedV * Input.GetAxis("Mouse Y");

                    transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);
                }
            }
            else
            {
                //Zoom in and out with Right Mouse
                if (Input.GetMouseButton(1))
                {
                    transform.Translate(0, 0, Input.GetAxisRaw("Mouse X") * zoomSpeed * .07f, Space.Self);
                }
            }
        }
    }
}