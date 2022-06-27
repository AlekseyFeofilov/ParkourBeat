using System;
using Game.Scripts.Map;
using UnityEngine;

namespace Game.Scripts.Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Beatmap beatmap;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private Jumper jumper;

        private Vector3 _normal = Vector3.up;
        private Vector3 _move;

        [SerializeField] private float horizontalSpeed = 10f;

        public bool isTrigger;
        private bool _isBottomTrigger;
        private bool _jumping;

        private Rigidbody _rigidbody;
        private int _capacitor;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _capacitor = 1;
        }

        private void Update()
        {
            //if true leftMove = 1 else 0
            var leftMove = Convert.ToInt32(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow));
            //if true rightMove = 1 else 0
            var rightMove = Convert.ToInt32(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow));

            if (Input.GetKey(KeyCode.Space) && isTrigger)
            {
                jumper.Jump();
            }

            _move += Vector3.forward * ((leftMove - rightMove) * horizontalSpeed * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            var time = beatmap.AudioTime;
            var x = (float)beatmap.timeline.GetPositionBySecond(time);
            _move += Vector3.right * (x - transform.position.x);

            switch (_jumping)
            {
                case true:
                    _capacitor++;
                    break;

                case false when !_isBottomTrigger:
                    for (var i = 0; i < _capacitor; i++)
                    {
                        _rigidbody.AddForce(0, -20, 0);
                    }

                    _capacitor = 1;
                    break;
            }

            Move(_move);
            _move = Vector3.zero;

            if (transform.position.y < -20) gameManager.EndGame(0);
        }

        public void CollisionEnter(string collisionTag)
        {
            _rigidbody.velocity = Vector3.zero;

            if (collisionTag != "Bottom")
            {
                gameManager.EndGame(0);
            }

            _isBottomTrigger = true;
        }

        public void CollisionExit(string collisionTag)
        {
            if (collisionTag != "Bottom") return;

            _isBottomTrigger = false;
        }

        public void OnBeginJump()
        {
            _capacitor = 0;
            _jumping = true;
            _rigidbody.useGravity = false;
        }

        public void OnEndJump()
        {
            _jumping = false;
            _rigidbody.useGravity = true;
        }

        private void Move(Vector3 direction)
        {
            var directionAlongSurface = GetProjection(direction.normalized);
            if (directionAlongSurface.x == 0) return;
            var elongation = direction.x / directionAlongSurface.x;
            transform.position += directionAlongSurface * elongation;
        }

        private Vector3 GetProjection(Vector3 direction)
        {
            return direction - Vector3.Dot(direction, _normal) * _normal;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_normal == -Vector3.right ||
                _normal == Vector3.forward ||
                _normal == -Vector3.forward
               )
            {
                gameManager.EndGame(0);
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            _normal = collision.contacts[0].normal;
            isTrigger = true;
        }

        private void OnCollisionExit()
        {
            isTrigger = false;
            _normal = Vector3.up;
        }
    }
}