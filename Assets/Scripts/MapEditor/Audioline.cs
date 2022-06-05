using System;
using MapEditor.Timestamp;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Object;
using VisualEffect.Point;

namespace MapEditor
{
    public class Audioline : MonoBehaviour
    {
        [SerializeField]
        private Timeline timeline;

        [SerializeField]
        private AudioSource songSource;
        
        [SerializeField]
        private AudioSource soundSource;
        
        [SerializeField]
        private GameObject wallPrefab;

        [SerializeField]
        private Camera player;
        
        [SerializeField]
        private SkyObject skyObject;
        
        private GameObject _wall;
        private int _lastBeat = -1;

        private void Awake()
        {
            timeline.AddBpmPoint(ITime.OfSecond(1.11), 82);

            timeline.AddSpeedPoint(ITime.Zero, 5);
            timeline.AddSpeedPoint(ITime.OfSecond(1), 1);
            timeline.AddSpeedPoint(ITime.OfBeat(33), 3);
            timeline.AddSpeedPoint(ITime.OfBeat(37), 1);
            timeline.AddSpeedPoint(ITime.OfBeat(41), 3);
            timeline.AddSpeedPoint(ITime.OfBeat(45), 0.5);
            timeline.AddSpeedPoint(ITime.OfBeat(49), 5);
            timeline.AddSpeedPoint(ITime.OfBeat(193), 1);

            timeline.AddEffectPoint(new VisualEffectPoint(
                ITime.Zero, 
                ITime.OfSecond(.5f),
                ITimingFunction.Ease,
                skyObject.SkyColor,
                Color.black
            ));
            timeline.AddEffectPoint(new VisualEffectPoint(
                ITime.Zero,
                ITime.OfSecond(.5f),
                ITimingFunction.Ease,
                skyObject.HorizonColor,
                Color.black
            ));

            timeline.AddEffectPoint(new VisualEffectPoint(
                ITime.OfBeat(33), 
                ITime.OfBeat(1),
                ITimingFunction.Ease,
                skyObject.HorizonColor,
                Color.magenta
            ));
            timeline.AddEffectPoint(new VisualEffectPoint(
                ITime.OfBeat(37), 
                ITime.OfBeat(1),
                ITimingFunction.Ease,
                skyObject.HorizonColor,
                new Color(.48f, 0, 1)
            ));
            timeline.AddEffectPoint(new VisualEffectPoint(
                ITime.OfBeat(41), 
                ITime.OfBeat(1),
                ITimingFunction.Ease,
                skyObject.HorizonColor,
                Color.blue
            ));
            timeline.AddEffectPoint(new VisualEffectPoint(
                ITime.OfBeat(45), 
                ITime.OfBeat(4),
                ITimingFunction.Linear,
                skyObject.HorizonColor,
                Color.black
            ));
            
            timeline.AddEffectPoint(new VisualEffectPoint(
                ITime.OfBeat(1), 
                ITime.OfBeat(7),
                ITimingFunction.Linear,
                skyObject.SkyColor,
                Color.yellow
            ));
            timeline.AddEffectPoint(new VisualEffectPoint(
                ITime.OfBeat(8), 
                ITime.OfBeat(8),
                ITimingFunction.Linear,
                skyObject.SkyColor,
                Color.black
            ));
            timeline.AddEffectPoint(new VisualEffectPoint(
                ITime.OfBeat(16), 
                ITime.OfBeat(8),
                ITimingFunction.Linear,
                skyObject.HorizonColor,
                Color.yellow
            ));
            timeline.AddEffectPoint(new VisualEffectPoint(
                ITime.OfBeat(24), 
                ITime.OfBeat(8),
                ITimingFunction.Linear,
                skyObject.HorizonColor,
                Color.black
            ));
            
            // Разгон
            for (int i = 49; i < 65; i++)
            {
                if ((i - 1) % 4 != 0)
                {
                    timeline.AddEffectPoint(new VisualEffectPoint(
                        ITime.OfBeat(i), 
                        ITime.Zero,
                        ITimingFunction.Linear,
                        skyObject.HorizonColor,
                        new Color(.27f, 0, .57f)
                    ));
                    timeline.AddEffectPoint(new VisualEffectPoint(
                        ITime.OfBeat(i), 
                        ITime.OfBeat(.5),
                        ITimingFunction.Linear,
                        skyObject.HorizonColor,
                        new Color(.17f, 0, .36f)
                    ));
                    timeline.AddEffectPoint(new VisualEffectPoint(
                        ITime.OfBeat(i + .5), 
                        ITime.Zero,
                        ITimingFunction.Linear,
                        skyObject.HorizonColor,
                        new Color(.27f, 0, .57f)
                    ));
                    timeline.AddEffectPoint(new VisualEffectPoint(
                        ITime.OfBeat(i + .5), 
                        ITime.OfBeat(.5),
                        ITimingFunction.Linear,
                        skyObject.HorizonColor,
                        new Color(.17f, 0, .36f)
                    ));
                }
            }
            
            for (int i = 49; i <= 65; i += 4)
            {
                timeline.AddEffectPoint(new VisualEffectPoint(
                    ITime.OfBeat(i), 
                    ITime.Zero,
                    ITimingFunction.Linear,
                    skyObject.HorizonColor,
                    Color.magenta
                ));
                timeline.AddEffectPoint(new VisualEffectPoint(
                    ITime.OfBeat(i), 
                    ITime.OfBeat(.5),
                    ITimingFunction.Linear,
                    skyObject.HorizonColor,
                    new Color(.17f, 0, .36f)
                ));
            }

            timeline.AddBpmPoint(ITime.OfBeat(65), 82 * 4);
            timeline.AddBpmPoint(ITime.OfBeat(193), 82 * 2);
        }

        private void Update()
        {
            double time;
            
            // Если музыка играет, то обновляем эффекты по времени музыки
            if (songSource.isPlaying) time = songSource.time;
            // Иначе обновляем эффекты по расположению игрока
            else time = timeline.GetSecondByPosition(player.transform.position.x);

            // Обновляем эффекты
            timeline.Move(time);

            // Далее только для режима с воспроизведением музыки
            if (!songSource.isPlaying) return;
            
            // Движем стену
            double x = timeline.GetPositionBySecond(time);
            Vector3 position = _wall.transform.position;
            _wall.transform.position = new Vector3((float) x, position.y, position.z);

            // Биты
            int beat = (int) Math.Floor(timeline.GetBeatBySecond(time));
            if (_lastBeat == beat) return;
            _lastBeat = beat;
            soundSource.Play();
        }

        public void PlayAudio()
        {
            StopAudio();
            Vector3 scale = wallPrefab.transform.localScale;
            
            _wall = Instantiate(wallPrefab, new Vector3(0, scale.y / 2, 0), Quaternion.identity);
            _wall.transform.parent = transform;

            _lastBeat = -1;
            songSource.Play();
        }

        public void StopAudio()
        {
            if (_wall == null) return;
            Destroy(_wall);
            _wall = null;
            
            songSource.Stop();
        }
    }
}
