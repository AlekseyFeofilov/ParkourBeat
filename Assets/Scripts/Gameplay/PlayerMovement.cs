using System;
using Mechanics;
using UnityEngine;

namespace Gameplay
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Beatmap.Beatmap beatmap;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Jumping jumping;

        private Vector3 _normal = Vector3.up;
        private Vector3 _move;
        
        [SerializeField] private float horizontalSpeed = 10f;
        [SerializeField] private float jumpForce = 100f;
         
        private bool _isTopTrigger;
        private bool _isBottomTrigger;
        private bool _isLeftTrigger;
        private bool _isRightTrigger;

        private Rigidbody _rigidbody;
        [SerializeField] private float jumpLenght;
        [SerializeField] private float jumpDuration;
        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            //if true leftMove = 1 else 0
            var leftMove = Convert.ToInt32(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow));
            //if true rightMove = 1 else 0
            var rightMove = Convert.ToInt32(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow));

            if (Input.GetKey(KeyCode.Space) && !IsFreeFall())
            {
                jumping.Jump();
            }
            
            _move += Vector3.forward * ((leftMove - rightMove) * horizontalSpeed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            var time = beatmap.AudioTime;
            var x = (float) beatmap.timeline.GetPositionBySecond(time);
            _move += Vector3.right * (x - transform.position.x);
            
            Move(_move);
            _move = Vector3.zero;

            if(transform.position.y < -20) gameManager.EndGame(0);
        }
        
        public void CollisionEnter(string collisionTag)
        {
            switch (collisionTag)
            {
                case "Top":
                    _isTopTrigger = true;
                    break;
                
                case "Bottom":
                    _isBottomTrigger = true;
                    break;
                
                case "Left":
                    _isLeftTrigger = true;
                    break;
                
                case "Right":
                    _isRightTrigger = true;
                    break;
                
                default:
                    throw new Exception("Unknown collider involved CollisionEnter (Player.cs)");
            }
        }

        public void CollisionExit(string collisionTag)
        {
            switch (collisionTag)
            {
                case "Top":
                    _isTopTrigger = false;
                    break;
                
                case "Bottom":
                    _isBottomTrigger = false;
                    break;
                
                case "Left":
                    _isLeftTrigger = false;
                    break;
                
                case "Right":
                    _isRightTrigger = false;
                    break;

                default:
                    throw new Exception("Unknown collider involved CollisionExit (Player.cs)");
            }
        }

        private bool IsFreeFall()
        {
            return Physics.gravity.y < 0 && !_isBottomTrigger;
        }

        private void Move(Vector3 direction)
        {
            var directionAlongSurface = GetProjection(direction.normalized);
            if(directionAlongSurface.x == 0) return;
            var elongation = direction.x / directionAlongSurface.x;
            transform.position += directionAlongSurface * elongation;
        }

        private Vector3 GetProjection(Vector3 direction)
        {
            return direction - Vector3.Dot(direction, _normal) * _normal;
        }

        private void OnCollisionEnter(Collision collision)
        {
            _normal = collision.contacts[0].normal;

            if (_normal == -Vector3.right)
            {
                gameManager.EndGame(0.1f);
            }
        }
    }
}