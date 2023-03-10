using System;
using Game.Scripts.Map;
using Game.Scripts.Map.Manager;
using Game.Scripts.Map.Timeline;
using Game.Scripts.Map.Timestamp;
using Game.Scripts.Map.VisualEffect.Function;
using Game.Scripts.Map.VisualEffect.Object;
using UnityEngine;

namespace Game.Scripts.MapTest
{
    /// <summary>
    /// TODO удалить в будущем
    /// </summary>
    [Obsolete]
    public class TestBeatmap : MonoBehaviour
    {
        [SerializeField] private EditableBeatmap beatmap;

        private Timeline Timeline => beatmap.timeline;
        private ObjectManager ObjectManager => beatmap.objectManager;
        private TriggerManager TriggerManager => beatmap.triggerManager;
        private SkyObject SkyObject => ObjectManager.skyObject;
        private HorizonObject HorizonObject => ObjectManager.horizonObject;
        
        public void Start()
        {
            Timeline.AddBpmPoint(MapTime.OfSecond(1.11), 82);

            Timeline.AddSpeedPoint(MapTime.Zero, 5);
            Timeline.AddSpeedPoint(MapTime.OfSecond(1), 1);
            Timeline.AddSpeedPoint(MapTime.OfBeat(33), 3);
            Timeline.AddSpeedPoint(MapTime.OfBeat(37), 1);
            Timeline.AddSpeedPoint(MapTime.OfBeat(41), 3);
            Timeline.AddSpeedPoint(MapTime.OfBeat(45), 0.5);
            Timeline.AddSpeedPoint(MapTime.OfBeat(49), 5);
            Timeline.AddSpeedPoint(MapTime.OfBeat(193), 1);

            TriggerManager.CreateEffectTrigger(
                MapTime.Zero, 
                MapTime.OfSecond(.5f),
                ITimingFunction.Ease,
                SkyObject.Color,
                Color.black
            );
            TriggerManager.CreateEffectTrigger(
                MapTime.Zero,
                MapTime.OfSecond(.5f),
                ITimingFunction.Ease,
                HorizonObject.Color,
                Color.black
            );

            TriggerManager.CreateEffectTrigger(
                MapTime.OfBeat(33), 
                MapTime.OfBeat(1),
                ITimingFunction.Ease,
                HorizonObject.Color,
                Color.magenta
            );
            TriggerManager.CreateEffectTrigger(
                MapTime.OfBeat(37), 
                MapTime.OfBeat(1),
                ITimingFunction.Ease,
                HorizonObject.Color,
                new Color(.48f, 0, 1)
            );
            TriggerManager.CreateEffectTrigger(
                MapTime.OfBeat(41), 
                MapTime.OfBeat(1),
                ITimingFunction.Ease,
                HorizonObject.Color,
                Color.blue
            );
            TriggerManager.CreateEffectTrigger(
                MapTime.OfBeat(45), 
                MapTime.OfBeat(4),
                ITimingFunction.Linear,
                HorizonObject.Color,
                Color.black
            );
            
            TriggerManager.CreateEffectTrigger(
                MapTime.OfBeat(1), 
                MapTime.OfBeat(7),
                ITimingFunction.Linear,
                SkyObject.Color,
                Color.yellow
            );
            TriggerManager.CreateEffectTrigger(
                MapTime.OfBeat(8), 
                MapTime.OfBeat(8),
                ITimingFunction.Linear,
                SkyObject.Color,
                Color.black
            );
            TriggerManager.CreateEffectTrigger(
                MapTime.OfBeat(16), 
                MapTime.OfBeat(8),
                ITimingFunction.Linear,
                HorizonObject.Color,
                Color.yellow
            );
            TriggerManager.CreateEffectTrigger(
                MapTime.OfBeat(24), 
                MapTime.OfBeat(8),
                ITimingFunction.Linear,
                HorizonObject.Color,
                Color.black
            );
            
            // Разгон
            for (int k = 0; k < 2; k++)
            {
                TriggerManager.CreateEffectTrigger(
                    MapTime.OfBeat(49 + k*16),
                    MapTime.Zero,
                    ITimingFunction.Linear,
                    HorizonObject.Color,
                    Color.magenta
                );
                TriggerManager.CreateEffectTrigger(
                    MapTime.OfBeat(49 + k*16),
                    MapTime.OfBeat(.9),
                    ITimingFunction.Linear,
                    HorizonObject.Color,
                    new Color(.17f, 0, .36f)
                ).AddZ(-1f);
                for (double i = 50; i < 53; i += 0.5)
                {
                    TriggerManager.CreateEffectTrigger(
                        MapTime.OfBeat(i + k*16),
                        MapTime.Zero,
                        ITimingFunction.Linear,
                        HorizonObject.Color,
                        new Color(.27f, 0, .57f)
                    );
                    TriggerManager.CreateEffectTrigger(
                        MapTime.OfBeat(i + k*16),
                        MapTime.OfBeat(.5),
                        ITimingFunction.Linear,
                        HorizonObject.Color,
                        new Color(.17f, 0, .36f)
                    ).AddZ(-1f);
                }

                TriggerManager.CreateEffectTrigger(
                    MapTime.OfBeat(53 + k*16),
                    MapTime.Zero,
                    ITimingFunction.Linear,
                    HorizonObject.Color,
                    new Color(.64f, .11f, .37f)
                );
                TriggerManager.CreateEffectTrigger(
                    MapTime.OfBeat(53 + k*16),
                    MapTime.OfBeat(.9),
                    ITimingFunction.Linear,
                    HorizonObject.Color,
                    new Color(.18f, .12f, .17f)
                ).AddZ(-1f);
                for (double i = 54; i < 57; i += 0.5)
                {
                    TriggerManager.CreateEffectTrigger(
                        MapTime.OfBeat(i + k*16),
                        MapTime.Zero,
                        ITimingFunction.Linear,
                        HorizonObject.Color,
                        new Color(.29f, .13f, .26f)
                    );
                    TriggerManager.CreateEffectTrigger(
                        MapTime.OfBeat(i + k*16),
                        MapTime.OfBeat(.5),
                        ITimingFunction.Linear,
                        HorizonObject.Color,
                        new Color(.18f, .12f, .17f)
                    ).AddZ(-1f);
                }

                TriggerManager.CreateEffectTrigger(
                    MapTime.OfBeat(57 + k*16),
                    MapTime.Zero,
                    ITimingFunction.Linear,
                    HorizonObject.Color,
                    new Color(.19f, .49f, .67f)
                );
                TriggerManager.CreateEffectTrigger(
                    MapTime.OfBeat(57 + k*16),
                    MapTime.OfBeat(.9),
                    ITimingFunction.Linear,
                    HorizonObject.Color,
                    new Color(.11f, .16f, .19f)
                ).AddZ(-1f);
                for (double i = 58; i < 61; i += 0.5)
                {
                    TriggerManager.CreateEffectTrigger(
                        MapTime.OfBeat(i + k*16),
                        MapTime.Zero,
                        ITimingFunction.Linear,
                        HorizonObject.Color,
                        new Color(.16f, .25f, .31f)
                    );
                    TriggerManager.CreateEffectTrigger(
                        MapTime.OfBeat(i + k*16),
                        MapTime.OfBeat(.5),
                        ITimingFunction.Linear,
                        HorizonObject.Color,
                        new Color(.11f, .16f, .19f)
                    ).AddZ(-1f);
                }

                TriggerManager.CreateEffectTrigger(
                    MapTime.OfBeat(61 + k*16),
                    MapTime.Zero,
                    ITimingFunction.Linear,
                    HorizonObject.Color,
                    new Color(.25f, .31f, .83f)
                );
                TriggerManager.CreateEffectTrigger(
                    MapTime.OfBeat(61 + k*16),
                    MapTime.OfBeat(.9),
                    ITimingFunction.Linear,
                    HorizonObject.Color,
                    new Color(.11f, .12f, .23f)
                ).AddZ(-1f);
                for (double i = 62; i < 65; i += 0.5)
                {
                    TriggerManager.CreateEffectTrigger(
                        MapTime.OfBeat(i + k*16),
                        MapTime.Zero,
                        ITimingFunction.Linear,
                        HorizonObject.Color,
                        new Color(.16f, .18f, .40f)
                    );
                    TriggerManager.CreateEffectTrigger(
                        MapTime.OfBeat(i + k*16),
                        MapTime.OfBeat(.5),
                        ITimingFunction.Linear,
                        HorizonObject.Color,
                        new Color(.11f, .12f, .23f)
                    ).AddZ(-1f);
                }
            }

            TriggerManager.CreateEffectTrigger(
                MapTime.OfBeat(81), 
                MapTime.OfBeat(2),
                ITimingFunction.Ease,
                HorizonObject.Color,
                Color.black
            );

            Timeline.AddBpmPoint(MapTime.OfBeat(81), 82 * 4);
            Timeline.AddBpmPoint(MapTime.OfBeat(145), 82 * 2);
        }
    }
}