using System;
using System.Collections;
using Game.Scripts.Map;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Disclaimer
{
    public class DisclaimerFade : MonoBehaviour
    {
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private Image image;
        [SerializeField] private float fadeDuration;
        [SerializeField] private float stayDuration;
        [SerializeField] private BeatmapManager beatmapManager;

        private void Start()
        {
            StartCoroutine(AnimateFade());
        }

        public IEnumerator AnimateFade()
        {
            yield return Animate(value =>
            {
                image.color = Color.black * (1 - value);
            }, false);
            yield return new WaitForSeconds(stayDuration);
            yield return Animate(value =>
            {
                image.color = Color.black * value;
            }, false);
            beatmapManager.LoadBeatmapInPlaymode();
        }

        public void AnimateFadeOut()
        {
            StartCoroutine(Animate(value =>
            {
                image.color = Color.black * value;
            }, false));
        }

        private IEnumerator Animate(Action<float> action, bool inversed)
        {
            double beginTime = Time.timeAsDouble;
            double elapsedTime;
            
            while ((elapsedTime = Time.timeAsDouble - beginTime) < fadeDuration)
            {
                double argument = inversed ? fadeDuration - elapsedTime / fadeDuration : elapsedTime / fadeDuration;
                float value = animationCurve.Evaluate((float) argument);
                action.Invoke(value);
                yield return null;
            }
            action.Invoke(1f);
        }
    }
}