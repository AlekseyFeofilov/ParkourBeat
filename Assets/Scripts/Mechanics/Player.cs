using System;
using UnityEngine;

namespace Mechanics
{
    //todo: Set normal axes xyz instead of xzy
    public class Player : MonoBehaviour
    {
        // ReSharper disable once InconsistentNaming
        public float speedX = 0.1f;
        public float jumpForce = 250f;
        public float bigJumpForce = 350f;
        public float pushForce = 10f;
        public MovingMode movingMode = MovingMode.Slip;

        private Vector3 _move;
        private Rigidbody _rigidBody;
        private MovingMode _movingMode;

        private bool _isTopTrigger;
        private bool _isBottomTrigger;
        
        public enum MovingMode
        {
            Slip,
            Flying,
            Gravitation,
        }
        
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();

            ChangeMovementMode(movingMode);
        }

        private bool IsFreeFall()
        {
            return  Physics.gravity.y < 0 && !_isBottomTrigger ||
                    Physics.gravity.y > 0 && !_isTopTrigger;
        }

        internal void CollisionEnter(string collisionTag)
        {
            switch (collisionTag)
            {
                case "Top":
                    _isTopTrigger = true;
                    break;
                
                case "Bottom":
                    _isBottomTrigger = true;
                    break;
                
                default:
                    throw new Exception("Unknown collider involved CollisionEnter (Player.cs)");
            }
        }

        internal void CollisionExit(string collisionTag)
        {
            switch (collisionTag)
            {
                case "Top":
                    _isTopTrigger = false;
                    break;
                
                case "Bottom":
                    _isBottomTrigger = false;
                    break;
                
                default:
                    throw new Exception("Unknown collider involved CollisionExit (Player.cs)");
            }
        }
        
        private void Update()
        {
            //if true leftMove = 1 else 0
            var leftMove = Convert.ToInt32(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow));
            //if true rightMove = 1 else 0
            var rightMove = Convert.ToInt32(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow));

            //rightMove is negative because this is directed in the negative direction of Z-axis
            _move = new Vector3(0, 0, leftMove - rightMove);

            switch (_movingMode)
            {
                case MovingMode.Flying:
                    var upMove = Convert.ToInt32(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
                    _move += new Vector3(0, upMove);
                    break;

                case MovingMode.Slip:
                    if (Input.GetKeyDown(KeyCode.Space) && !IsFreeFall())
                    {
                        Jump();
                    }

                    break;

                case MovingMode.Gravitation:
                    if (IsFreeFall()) return;

                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                    {
                        SetUpGravity();
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                    {
                        SetDownGravity();
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FixedUpdate()
        {
            MoveForward();

            if (_movingMode == MovingMode.Flying)
            {
                Fly(_move);
            }
            else
            {
                MoveHorizontal(_move);
            }
        }

        private void ChangeMovementMode(MovingMode newMovingMode)
        {
            _movingMode = newMovingMode;

            switch (newMovingMode)
            {
                case MovingMode.Slip:
                    SetSlipMode();
                    break;

                case MovingMode.Gravitation:
                    SetGravityMode();
                    break;

                case MovingMode.Flying:
                    SetFlyingMode();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(newMovingMode), newMovingMode, null);
            }
        }

        private void SetSlipMode()
        {
            SetDefaultGravity();
            SetDefaultPushForce();
        }

        private void SetGravityMode()
        {
            SetDownGravity();
            SetDefaultPushForce();
        }

        private void SetFlyingMode()
        {
            SetLowGravity();
            SetLowPushForce();
        }

        private void Fly(Vector3 direction)
        {
            if (direction.y != 0) _rigidBody.velocity = Vector3.zero;

            transform.Translate(direction * pushForce);
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

        private void SetDefaultPushForce()
        {
            pushForce = 10f;
        }

        private void SetLowPushForce()
        {
            pushForce = 0.1f;
        }

        private void MoveForward()
        {
            transform.Translate(speedX, 0, 0);
        }

        private void MoveHorizontal(Vector3 direction)
        {
            transform.Translate(direction * (pushForce * Time.deltaTime));
        }

        private void Jump()
        {
            _rigidBody.AddForce(transform.up * jumpForce);
        }
    }
}