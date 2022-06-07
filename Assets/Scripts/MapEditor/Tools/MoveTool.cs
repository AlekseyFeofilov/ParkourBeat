using System;
using UnityEngine;

namespace MapEditor.Tools
{
    public class MoveTool : MonoBehaviour
    {
        private bool _activated;
        private Camera _camera;

        [SerializeField] private float speedBooster = 0.1f;

        private void OnMouseDown()
        {
            _activated = true;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var cameraPosition = _camera.transform.position;
            var position = transform.position;
            var scale = (float) Math.Sqrt(
                Math.Pow(cameraPosition.x - position.x, 2) +
                Math.Pow(cameraPosition.y - position.y, 2) +
                Math.Pow(cameraPosition.z - position.z, 2)
            ) / 3;
            // ReSharper disable once Unity.InefficientPropertyAccess
            transform.localScale = new Vector3(scale, scale, scale);

            if (!Input.GetMouseButton(0) || !_activated)
            {
                if (_activated) _activated = false;
                return;
            }

            var speed = speedBooster * (Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y"));
            Move(speed);
        }

        private void Move(float speed)
        {
            switch (tag)
            {
                case "OX":
                    MoveOx(speed);
                    break;

                case "OY":
                    MoveOy(speed);
                    break;

                case "OZ":
                    MoveOz(speed);
                    break;
            }
        }

        private static void MoveOx(float speed)
        {
            MainTools.Move(new Vector3(speed, 0));
        }

        private static void MoveOy(float speed)
        {
            MainTools.Move(new Vector3(0, speed));
        }

        private static void MoveOz(float speed)
        {
            MainTools.Move(new Vector3(0, 0, speed));
        }
    }
}