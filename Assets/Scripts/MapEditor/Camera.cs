using UnityEngine;

namespace MapEditor
{
    public class Camera : MonoBehaviour
    {
        private const float MoveSpeed = 5f;
        private const float ShiftMultiplier = 2f;

        private const float LookSpeedH = 2.5f;
        private const float LookSpeedV = 2.5f;
        private const float ZoomSpeed = 2.5f;
        private const float DragSpeed = 50f;

        private float _yaw = 0f;
        private float _pitch = 0f;

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
            Vector3 move;

            if (Input.GetKey(KeyCode.E))
                move = Vector3.forward;
            else if (Input.GetKey(KeyCode.Q))
                move = -Vector3.forward;
            else 
                move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            var speed = MoveSpeed * (Input.GetKey(KeyCode.LeftShift) ? ShiftMultiplier : 1f) * Time.deltaTime;
            move *= speed;
            transform.Translate(move);

            //drag camera around with Middle Mouse
            if (Input.GetMouseButton(2))
            {
                transform.Translate(-Input.GetAxisRaw("Mouse X") * Time.deltaTime * DragSpeed,
                    -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * DragSpeed, 0);
            }

            //Zoom in and out with Mouse Wheel
            transform.Translate(
                0,
                0,
                Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed *
                (Input.GetKey(KeyCode.LeftShift) ? ShiftMultiplier : 1f),
                Space.Self);

            if (!Input.GetKey(KeyCode.LeftAlt))
            {
                //Look around with Right Mouse
                if (Input.GetMouseButton(1))
                {
                    _yaw += LookSpeedH * Input.GetAxis("Mouse X");
                    _pitch -= LookSpeedV * Input.GetAxis("Mouse Y");

                    transform.eulerAngles = new Vector3(_pitch, _yaw, 0f);
                }
            }
            else
            {
                //Zoom in and out with Right Mouse
                if (Input.GetMouseButton(1))
                {
                    transform.Translate(0, 0, Input.GetAxisRaw("Mouse X") * ZoomSpeed * .07f, Space.Self);
                }
            }
        }
    }
}