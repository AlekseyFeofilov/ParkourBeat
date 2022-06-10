using System;
using MapEditor.Timestamp;
using MapEditor.Trigger;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Object;
using VisualEffect.Property;

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
        private Camera player;
        
        [SerializeField]
        private SkyObject skyObject;
        
        [SerializeField]
        private GameObject wallPrefab;

        [SerializeField]
        private GameObject triggerPrefab;
        
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

            CreateEffectTrigger(
                ITime.Zero, 
                ITime.OfSecond(.5f),
                ITimingFunction.Ease,
                skyObject.SkyColor,
                Color.black
            );
            CreateEffectTrigger(
                ITime.Zero,
                ITime.OfSecond(.5f),
                ITimingFunction.Ease,
                skyObject.HorizonColor,
                Color.black
            );

            CreateEffectTrigger(
                ITime.OfBeat(33), 
                ITime.OfBeat(1),
                ITimingFunction.Ease,
                skyObject.HorizonColor,
                Color.magenta
            );
            CreateEffectTrigger(
                ITime.OfBeat(37), 
                ITime.OfBeat(1),
                ITimingFunction.Ease,
                skyObject.HorizonColor,
                new Color(.48f, 0, 1)
            );
            CreateEffectTrigger(
                ITime.OfBeat(41), 
                ITime.OfBeat(1),
                ITimingFunction.Ease,
                skyObject.HorizonColor,
                Color.blue
            );
            CreateEffectTrigger(
                ITime.OfBeat(45), 
                ITime.OfBeat(4),
                ITimingFunction.Linear,
                skyObject.HorizonColor,
                Color.black
            );
            
            CreateEffectTrigger(
                ITime.OfBeat(1), 
                ITime.OfBeat(7),
                ITimingFunction.Linear,
                skyObject.SkyColor,
                Color.yellow
            );
            CreateEffectTrigger(
                ITime.OfBeat(8), 
                ITime.OfBeat(8),
                ITimingFunction.Linear,
                skyObject.SkyColor,
                Color.black
            );
            CreateEffectTrigger(
                ITime.OfBeat(16), 
                ITime.OfBeat(8),
                ITimingFunction.Linear,
                skyObject.HorizonColor,
                Color.yellow
            );
            CreateEffectTrigger(
                ITime.OfBeat(24), 
                ITime.OfBeat(8),
                ITimingFunction.Linear,
                skyObject.HorizonColor,
                Color.black
            );
            
            // Разгон
            for (int k = 0; k < 2; k++)
            {
                CreateEffectTrigger(
                    ITime.OfBeat(49 + k*16),
                    ITime.Zero,
                    ITimingFunction.Linear,
                    skyObject.HorizonColor,
                    Color.magenta
                );
                CreateEffectTrigger(
                    ITime.OfBeat(49 + k*16),
                    ITime.OfBeat(.9),
                    ITimingFunction.Linear,
                    skyObject.HorizonColor,
                    new Color(.17f, 0, .36f)
                ).AddZ(-1f);
                for (double i = 50; i < 53; i += 0.5)
                {
                    CreateEffectTrigger(
                        ITime.OfBeat(i + k*16),
                        ITime.Zero,
                        ITimingFunction.Linear,
                        skyObject.HorizonColor,
                        new Color(.27f, 0, .57f)
                    );
                    CreateEffectTrigger(
                        ITime.OfBeat(i + k*16),
                        ITime.OfBeat(.5),
                        ITimingFunction.Linear,
                        skyObject.HorizonColor,
                        new Color(.17f, 0, .36f)
                    ).AddZ(-1f);
                }

                CreateEffectTrigger(
                    ITime.OfBeat(53 + k*16),
                    ITime.Zero,
                    ITimingFunction.Linear,
                    skyObject.HorizonColor,
                    new Color(.64f, .11f, .37f)
                );
                CreateEffectTrigger(
                    ITime.OfBeat(53 + k*16),
                    ITime.OfBeat(.9),
                    ITimingFunction.Linear,
                    skyObject.HorizonColor,
                    new Color(.18f, .12f, .17f)
                ).AddZ(-1f);
                for (double i = 54; i < 57; i += 0.5)
                {
                    CreateEffectTrigger(
                        ITime.OfBeat(i + k*16),
                        ITime.Zero,
                        ITimingFunction.Linear,
                        skyObject.HorizonColor,
                        new Color(.29f, .13f, .26f)
                    );
                    CreateEffectTrigger(
                        ITime.OfBeat(i + k*16),
                        ITime.OfBeat(.5),
                        ITimingFunction.Linear,
                        skyObject.HorizonColor,
                        new Color(.18f, .12f, .17f)
                    ).AddZ(-1f);
                }

                CreateEffectTrigger(
                    ITime.OfBeat(57 + k*16),
                    ITime.Zero,
                    ITimingFunction.Linear,
                    skyObject.HorizonColor,
                    new Color(.19f, .49f, .67f)
                );
                CreateEffectTrigger(
                    ITime.OfBeat(57 + k*16),
                    ITime.OfBeat(.9),
                    ITimingFunction.Linear,
                    skyObject.HorizonColor,
                    new Color(.11f, .16f, .19f)
                ).AddZ(-1f);
                for (double i = 58; i < 61; i += 0.5)
                {
                    CreateEffectTrigger(
                        ITime.OfBeat(i + k*16),
                        ITime.Zero,
                        ITimingFunction.Linear,
                        skyObject.HorizonColor,
                        new Color(.16f, .25f, .31f)
                    );
                    CreateEffectTrigger(
                        ITime.OfBeat(i + k*16),
                        ITime.OfBeat(.5),
                        ITimingFunction.Linear,
                        skyObject.HorizonColor,
                        new Color(.11f, .16f, .19f)
                    ).AddZ(-1f);
                }

                CreateEffectTrigger(
                    ITime.OfBeat(61 + k*16),
                    ITime.Zero,
                    ITimingFunction.Linear,
                    skyObject.HorizonColor,
                    new Color(.25f, .31f, .83f)
                );
                CreateEffectTrigger(
                    ITime.OfBeat(61 + k*16),
                    ITime.OfBeat(.9),
                    ITimingFunction.Linear,
                    skyObject.HorizonColor,
                    new Color(.11f, .12f, .23f)
                ).AddZ(-1f);
                for (double i = 62; i < 65; i += 0.5)
                {
                    CreateEffectTrigger(
                        ITime.OfBeat(i + k*16),
                        ITime.Zero,
                        ITimingFunction.Linear,
                        skyObject.HorizonColor,
                        new Color(.16f, .18f, .40f)
                    );
                    CreateEffectTrigger(
                        ITime.OfBeat(i + k*16),
                        ITime.OfBeat(.5),
                        ITimingFunction.Linear,
                        skyObject.HorizonColor,
                        new Color(.11f, .12f, .23f)
                    ).AddZ(-1f);
                }
            }

            CreateEffectTrigger(
                ITime.OfBeat(81), 
                ITime.OfBeat(2),
                ITimingFunction.Ease,
                skyObject.HorizonColor,
                Color.black
            );

            timeline.AddBpmPoint(ITime.OfBeat(81), 82 * 4);
            timeline.AddBpmPoint(ITime.OfBeat(145), 82 * 2);
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

        /// <summary>
        /// Синхронизирует время музыки с расположением игрока
        /// </summary>
        public void SynchronizeAudioWithPosition()
        {
            double time = timeline.GetSecondByPosition(player.transform.position.x);
            int beat = (int) Math.Floor(timeline.GetBeatBySecond(time));

            if (beat >= 0)
            {
                time = timeline.GetSecondByBeat(beat);
            }

            songSource.time = Mathf.Max(0, (float) time);
        }

        /// <summary>
        /// Обнуляет время музыки до нуля
        /// </summary>
        public void ResetAudioTimestamp()
        {
            songSource.time = 0;
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
        
        /*private SpeedTimestamp CreateSpeedTrigger(
            ITime time, 
            double second,
            double speed,
            double x)
        {
            var position = transform.position;
            Vector3 vectorPos = new((float) x, position.y, position.z);
            GameObject gameObj = Instantiate(triggerPrefab, vectorPos, Quaternion.identity);
            gameObj.transform.parent = transform;
            
            SpeedTimestamp timestamp = new()
            {
                Timeline = this,
                Time = time,
                Second = second,
                Speed = speed,
                Position = x
            };
            return timestamp;
        }*/
        
        /*private BpmTimestamp CreateBpmTrigger(
            double second,
            double bpm,
            double beat)
        {
            float x = (float) GetPositionBySecond(second);
            var position = transform.position;
            Vector3 vectorPos = new(x, position.y, position.z);
            GameObject gameObj = Instantiate(triggerPrefab, vectorPos, Quaternion.identity);
            gameObj.transform.parent = transform;
            
            BpmTimestamp timestamp = new()
            {
                Timeline = this,
                Second = second,
                Bpm = bpm,
                Beat = beat
            };
            return timestamp;
        }*/
        
        private EffectTrigger CreateEffectTrigger(
            ITime time,
            ITime duration,
            ITimingFunction function,
            IVisualProperty property,
            object state)
        {
            GameObject gameObj = Instantiate(triggerPrefab, Vector3.zero * 1, Quaternion.identity);
            gameObj.transform.parent = transform;

            EffectTimestamp timestamp = 
                timeline.AddEffectPoint(time, duration, function, property, state);
            
            EffectTrigger trigger = gameObj.AddComponent<EffectTrigger>();
            trigger.Timeline = timeline;
            trigger.Timestamp = timestamp;
                
            return trigger;
        }
    }
}
