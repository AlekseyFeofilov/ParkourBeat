using System.Collections;
using UnityEngine;

namespace Beatmap
{
    public class PlayableBeatmap : Beatmap
    {
        [SerializeField] private GameObject player;

        private float _lastX;
        
        private void FixedUpdate()
        {
            if (!songSource.isPlaying) return;
            double time = AudioTime;
            
            // Обновляем эффекты
            timeline.Move(time);
            
            // Двигаем игрока по x
            float x = (float) timeline.GetPositionBySecond(time);
            Vector3 pos = player.transform.position;
            player.transform.position = new Vector3(x, pos.y, pos.z);
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
    }
}