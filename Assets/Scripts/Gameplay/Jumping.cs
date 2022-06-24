using System;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class Jumping : MonoBehaviour
    {
        [SerializeField] private AnimationCurve animation;
        [SerializeField] private Transform jumper;
        [SerializeField] private float jumpHeight = 0.5f;
        [SerializeField] private float deltaTime = 0.5f;
        private Coroutine _lastAnimation;
        private bool _stopAnimation;
        private float _startHeight;

        public void Jump()
        {
            _startHeight = jumper.position.y;
            // ReSharper disable once Unity.InefficientPropertyAccess
            PlayAnimation(deltaTime);
        }
        
        private void PlayAnimation(float duration)
        {
            Play(duration, progress =>
            {
                // ReSharper disable once NotAccessedVariable
                var position = jumper.position;
                position = new Vector3(position.x, jumpHeight * animation.Evaluate(progress) + _startHeight, position.z);
                jumper.position = position;
                return 0;
            });
        }

        private void Play(float duration, Func<float, float> body)
        {
            if (_lastAnimation != null) StopCoroutine(_lastAnimation);

            _stopAnimation = false;
            _lastAnimation = StartCoroutine(GetAnimation(duration, body));
        }

        private IEnumerator GetAnimation(float duration, Func<float, float> body)
        {
            var expiredSecond = 0f;
            var progress = 0f;

            while (progress < 1 && !_stopAnimation)
            {
                expiredSecond += Time.deltaTime;
                progress = expiredSecond / duration;
                body.Invoke(progress);

                yield return null;
            }
        }
    }
}