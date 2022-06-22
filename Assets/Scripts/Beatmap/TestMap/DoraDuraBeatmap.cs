using Beatmap.Object;
using MapEditor;
using MapEditor.Timestamp;
using MapEditor.Trigger;
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
                SkyObject.SkyColor,
                Color.black
            );
            TriggerManager.CreateEffectTrigger(
                MapTime.Zero,
                MapTime.Zero,
                ITimingFunction.Linear,
                SkyObject.HorizonColor,
                Color.black
            );
        }
    }
}