using System;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Object;

namespace Mechanics
{
    public class Player : MonoBehaviour
    {
        private GamaManager _gamaManager;

        public float speedX = 0.1f;
        private float _speedZ;
        public float jumpForce = 50000f;

        private float _borderSize;
        private Rigidbody _rigidBody;

        private void Start()
        {
            _gamaManager = FindObjectOfType<GamaManager>();
            _borderSize = GetComponent<Collider>().bounds.size.y;
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Move3();

            if (IsDead())
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                _gamaManager.EndGame();
            }
        }

        private void Move3()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                AddForceLeft();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                AddForceRight();
            }
            else
            {
                SetDefaultForce();
            }

            Move2();

            if (Input.GetKeyDown(KeyCode.Space) && Math.Abs(transform.position.y) < 0.01f)
            {
                Jump();
            }
        }

        private void AddForceLeft()
        {
            _speedZ = _borderSize;
        }

        private void AddForceRight()
        {
            _speedZ = -_borderSize;
        }

        private void SetDefaultForce()
        {
            _speedZ = 0f;
        }

        private void Move2()
        {
            transform.Translate(speedX, 0, _speedZ);
        }

        private void Jump()
        {
            _rigidBody.AddForce(transform.up * jumpForce);
        }

        private bool IsDead()
        {
            return transform.position.y < -3f;
        }
    }
}