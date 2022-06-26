using System;
using System.Collections;
using Game.Scripts.Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    /// <summary>
    /// Класс для плавного появления и исчезнования сцены (через затемнение)
    /// </summary>
    public class SceneFade : MonoBehaviour
    {
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private Image image;
        [SerializeField] private float duration;

        public void AnimateFadeIn(SceneLoadEvent @event)
        {
            @event.Coroutines.Add(StartCoroutine(Animate(value =>
            {
                image.color = Color.black * (1 - value);
            }, false)));
        }

        public void AnimateFadeOut(SceneUnloadEvent @event)
        {
            @event.Coroutines.Add(StartCoroutine(Animate(value =>
            {
                image.color = Color.black * value;
            }, false)));
        }

        private IEnumerator Animate(Action<float> action, bool inversed)
        {
            double beginTime = Time.timeAsDouble;
            double elapsedTime;
            
            while ((elapsedTime = Time.timeAsDouble - beginTime) < duration)
            {
                double argument = inversed ? duration - elapsedTime / duration : elapsedTime / duration;
                float value = animationCurve.Evaluate((float) argument);
                action.Invoke(value);
                yield return null;
            }
            action.Invoke(1f);
        }
    }
}