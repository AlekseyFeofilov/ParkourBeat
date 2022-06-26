﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Scripts.Map
{
    public class PlayableBeatmap : Beatmap
    {
        [SerializeField] private Image fade;
        
        private float _lastX;
        private bool ended;

        protected override void Start()
        {
            base.Start();
            RenderSettings.fogDensity = Fog;
        }

        private void FixedUpdate()
        {
            if (!songSource.isPlaying) return;
            double time = AudioTime;
            
            // Обновляем эффекты
            timeline.Move(time);
            
            // Двигаем игрока по x
            /*var x = (float) timeline.GetPositionBySecond(time);
            Vector3 pos = player.transform.position;
            player.transform.position = new Vector3(x, pos.y, pos.z);*/
            if (time > End && !ended)
            {
                ended = true;
                StartCoroutine(FadeOut());
            }
        }

        private IEnumerator FadeOut()
        {
            for (int i = 0; i < 500; i++)
            {
                songSource.volume = (500 - i) / 500f;
                fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, i / 500f);
                yield return new WaitForSeconds(0.01f);
            }
            songSource.Stop();
            SceneManager.LoadScene("Level Menu");
        }
        
        protected override void OnInitialized()
        {
            StartCoroutine(BeginGame());
        }

        private IEnumerator BeginGame()
        {
            yield return new WaitForSeconds(1);
            PlayAudio();
        }

        public override void PlayAudio()
        {
            StopAudio();
            songSource.time = 0f;
            songSource.Play();
        }

        public override void StopAudio()
        {
            songSource.Stop();
        }
        
        public void PauseAudio()
        {
            songSource.pitch = 0f;
        }

        public void ContinueAudio()
        {
            songSource.pitch = 1f;
        }
    }
}