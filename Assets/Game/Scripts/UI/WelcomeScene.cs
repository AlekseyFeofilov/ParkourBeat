using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class WelcomeScene : MonoBehaviour
    {
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private Image image;
        [SerializeField] private float fadeDuration;
        [SerializeField] private float stayDuration;

        private bool flag;

        private void Start()
        {
            image.color = Color.black;
            StartCoroutine(AnimateFade());
        }

        private IEnumerator AnimateFade()
        {
            yield return Animate(value => { image.color = Color.black * (1 - value); }, false);
            yield return new WaitForSeconds(stayDuration);
            yield return Animate(value => { image.color = Color.black * value; }, false);
            SceneManager.LoadScene("Main Menu");
        }

        public void AnimateFadeOut()
        {
            StartCoroutine(Animate(value => { image.color = Color.black * value; }, false));
        }

        private IEnumerator Animate(Action<float> action, bool inversed)
        {
            double beginTime = Time.timeAsDouble;
            double elapsedTime;

            while ((elapsedTime = Time.timeAsDouble - beginTime) < fadeDuration)
            {
                double argument = inversed ? fadeDuration - elapsedTime / fadeDuration : elapsedTime / fadeDuration;
                float value = animationCurve.Evaluate((float)argument);
                action.Invoke(value);
                yield return null;
            }

            action.Invoke(1f);
        }

        private void Update()
        {
            //показываем "компанию"
            if (Input.anyKey && !flag)
            {
                flag = true;
                SceneManager.LoadScene("Main Menu");
            }
        }
    }
}