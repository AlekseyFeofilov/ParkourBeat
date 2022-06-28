using System;
using System.Collections;
using UnityEngine;

namespace Game.Scripts.Gameplay.Player
{
    public class Jumper : MonoBehaviour
    {
        [SerializeField] private AnimationCurve animation;
        [SerializeField] private PlayerMovement player;
        private Transform _jumper;
        
        [SerializeField] private float jumpHeight = 0.5f;
        [SerializeField] private float deltaTime = 0.5f;
        
        private Coroutine _lastAnimation;
        private float _startHeight;

        private void Start()
        {
            _jumper = player.transform;
        }

        public void Jump()
        {
            player.OnBeginJump();
            _startHeight = _jumper.position.y;
            // ReSharper disable once Unity.InefficientPropertyAccess
            PlayAnimation(deltaTime);
        }
        
        private void PlayAnimation(float duration)
        {
            Play(duration, progress =>
            {
                // ReSharper disable once NotAccessedVariable
                var position = _jumper.position;
                position = new Vector3(position.x, jumpHeight * animation.Evaluate(progress) + _startHeight, position.z);
                _jumper.position = position;
                return 0;
            });
        }

        private void Play(float duration, Func<float, float> body)
        {
            if (_lastAnimation != null) StopCoroutine(_lastAnimation);
            
            _lastAnimation = StartCoroutine(GetAnimation(duration, body));
        }

        private IEnumerator GetAnimation(float duration, Func<float, float> body)
        {
            var expiredSecond = 0f;
            var progress = 0f;

            while (progress < 1 && (!player.isTrigger || progress < 0.02))
            {
                expiredSecond += Time.deltaTime;
                progress = expiredSecond / duration;
                body.Invoke(progress);

                yield return null;
            }
            
            player.OnEndJump();
        }
    }
}