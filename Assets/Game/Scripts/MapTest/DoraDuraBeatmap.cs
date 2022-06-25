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
    public class DoraDuraBeatmap : MonoBehaviour
    {
        [SerializeField] private EditableBeatmap beatmap;

        private Timeline Timeline => beatmap.timeline;
        private ObjectManager ObjectManager => beatmap.objectManager;
        private TriggerManager TriggerManager => beatmap.triggerManager;
        private SkyObject SkyObject => ObjectManager.skyObject;
        private HorizonObject HorizonObject => ObjectManager.horizonObject;

        public void Start()
        {
            Timeline.AddBpmPoint(MapTime.OfSecond(0.33), 120);
            Timeline.AddBpmPoint(MapTime.OfBeat(44), 240);

            Timeline.AddSpeedPoint(MapTime.Zero, 2);
            Timeline.AddSpeedPoint(MapTime.OfBeat(44), 5);

            TriggerManager.CreateEffectTrigger(
                MapTime.Zero,
                MapTime.Zero,
                ITimingFunction.Linear,
                SkyObject.Color,
                Color.black
            );
            TriggerManager.CreateEffectTrigger(
                MapTime.Zero,
                MapTime.Zero,
                ITimingFunction.Linear,
                HorizonObject.Color,
                Color.black
            );
        }
    }
}