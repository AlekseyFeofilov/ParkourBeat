using UnityEngine;

namespace MapEditor
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float moveSpeedY = 0.03f;
        [SerializeField] private float controlMultiplier = 2f;

        [SerializeField] private float rotateSpeedH = 2.5f;
        [SerializeField] private float rotateSpeedV = 2.5f;
        [SerializeField] private float zoomSpeed = 2.5f;
        [SerializeField] private float dragSpeed = 50f;

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
            //Minecraft shift-space movement 
            var positiveMoveY = Input.GetKey(KeyCode.Space) ? moveSpeedY : 0;
            var negativeMoveY = Input.GetKey(KeyCode.LeftShift) ? -moveSpeedY : 0;
            var moveY = (positiveMoveY + negativeMoveY) * (Input.GetKey(KeyCode.LeftControl) ? controlMultiplier : 1f);
            transform.position += Vector3.up * moveY;
            
            //Zoom and Move with Keyboard
            var moveXZ = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            
            var speed = moveSpeed * (Input.GetKey(KeyCode.LeftControl) ? controlMultiplier : 1f) * Time.deltaTime;
            moveXZ *= speed;

            transform.Translate(moveXZ);

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
                (Input.GetKey(KeyCode.LeftControl) ? controlMultiplier : 1f),
                Space.Self);

            if (!Input.GetKey(KeyCode.LeftAlt))
            {
                //Look around with Right Mouse
                if (Input.GetMouseButton(1))
                {
                    _yaw += rotateSpeedH * Input.GetAxis("Mouse X");
                    _pitch -= rotateSpeedV * Input.GetAxis("Mouse Y");

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