using System;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Object;

namespace Mechanics
{
    //todo: Set normal axes xyz instead of xzy
    public class Player : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        private const float _speedX = 0.1f;
        private Vector3 _smoothMovement;

        private const float JumpForce = 500f;
        private const float BigJumpForce = 700f;
        private const float PushForce = 10f;

        private float _borderSize;

        private Rigidbody _rigidBody;
        private Collider _collider;

        private bool _isFreeFall;
        private MovingType _movingType;

        private enum MovingType
        {
            Slip,
            Flying,
            Gravitation,
        }

        private void OnCollisionEnter()
        {
            _isFreeFall = false;
        }

        private void OnCollisionExit()
        {
            _isFreeFall = true;
        }

        private void Start()
        {
            _collider = GetComponent<Collider>();
            _borderSize = _collider.bounds.size.y;
            _rigidBody = GetComponent<Rigidbody>();
            
            ChangeMovementType(MovingType.Slip);
        }

        private void Update()
        {
            switch (_movingType)
            {
                case MovingType.Flying:
                    //todo: remove inertia
                    
                    _smoothMovement = new Vector3(
                        0,
                        Math.Max(0, Input.GetAxis("Vertical")),
                        -Input.GetAxis("Horizontal")
                    );
                    break;
                
                case MovingType.Slip:
                    Move3();
                    break;
                
                case MovingType.Gravitation:
                    MoveWithGravity();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FixedUpdate()
        {
            MoveForward();

            if (_movingType == MovingType.Flying)
            {
                Fly(_smoothMovement);
            }
        }

        private void ChangeMovementType(MovingType movingType)
        {
            _movingType = movingType;

            switch (movingType)
            {
                case MovingType.Slip:
                    SetDefaultGravity();
                    break;
                
                case MovingType.Gravitation:
                    break;
                
                case MovingType.Flying:
                    SetLowGravity();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(movingType), movingType, null);
            }
        }

        private void Fly(Vector3 direction)
        {
            _rigidBody.AddForce(direction * PushForce);
        }

        private void MoveWithGravity()
        {
            if (!_isFreeFall)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                {
                    SetUpGravity();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                {
                    SetDownGravity();
                }
            }

            Move2();
        }

        private static void SetDownGravity()
        {
            Physics.gravity = new Vector3(0, -40f, 0);
        }

        private static void SetUpGravity()
        {
            Physics.gravity = new Vector3(0, 40f, 0);
        }

        private static void SetDefaultGravity()
        {
            Physics.gravity = new Vector3(0, -20f, 0);
        }
        
        private static void SetLowGravity()
        {
            Physics.gravity = new Vector3(0, -5f, 0);
        }

        private void Move3()
        {
            Move2();

            if (Input.GetKeyDown(KeyCode.Space) && !_isFreeFall)
            {
                Jump();
            }
        }

        private void MoveLeft()
        {
            transform.Translate(0, 0, _borderSize);
        }

        private void MoveRight()
        {
            transform.Translate(0, 0, -_borderSize);
        }

        private void MoveForward()
        {
            transform.Translate(_speedX, 0, 0);
        }

        private void Move2()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                MoveLeft();
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                MoveRight();
            }
        }

        private void Jump()
        {
            _rigidBody.AddForce(transform.up * JumpForce);
        }

        private void BigJump()
        {
            _rigidBody.AddForce(transform.up * JumpForce * 2);
        }
    }
}