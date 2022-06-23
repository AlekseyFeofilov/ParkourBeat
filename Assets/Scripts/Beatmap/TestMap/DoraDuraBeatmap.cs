﻿using Beatmap.Object;
using Beatmap.Trigger;
using MapEditor;
using MapEditor.Timestamp;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Object;

namespace Beatmap.TestMap
{
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