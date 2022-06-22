using System.Collections.Generic;
using System.Linq;
using Beatmap.Object;
using DataStructures.BiDictionary;
using MapEditor.Timestamp;
using Serialization.Data;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Object;
using VisualEffect.Property;

namespace MapEditor.Trigger
{
    public class TriggerManager : MonoBehaviour
    {
        [SerializeField] private Timeline timeline;
        [SerializeField] private ObjectManager objectManager;
        [SerializeField] private EffectTrigger effectTriggerPrefab;

        private readonly List<EffectTrigger> _effectTriggers = new();

        public EffectTrigger CreateEffectTrigger(
            MapTime beginTime,
            MapTime duration,
            ITimingFunction function,
            IVisualProperty property,
            object state)
        {
            EffectTrigger trigger = CreateEffectTrigger(beginTime, duration, function, property.Parent);
            trigger.AddProperty(property, state);
            return trigger;
        }

        public EffectTrigger CreateEffectTrigger(
            MapTime beginTime,
            MapTime duration,
            ITimingFunction function,
            MonoObject obj)
        {
            GameObject gameObj = Instantiate(effectTriggerPrefab.gameObject, Vector3.zero * 1, Quaternion.identity);
            gameObj.transform.parent = transform;

            TimeUnit endUnit = beginTime.Unit == duration.Unit ? duration.Unit : TimeUnit.Second;
            double endValue = beginTime.Unit == duration.Unit 
                ? beginTime.Value + duration.Value 
                : beginTime.ToSecond(timeline) + duration.ToSecond(timeline);
            MapTime endTime = new MapTime(endUnit, endValue);

            EffectTrigger trigger = gameObj.GetComponent<EffectTrigger>();
            trigger.Object = obj;
            trigger.TimingFunction = function;
            trigger.BeginTime = beginTime;
            trigger.EndTime = endTime;
            trigger.Timeline = timeline;

            _effectTriggers.Add(trigger);
            
            return trigger;
        }

        public void RemoveEffectTrigger(EffectTrigger trigger)
        {
            Destroy(trigger.gameObject);
            _effectTriggers.Remove(trigger);
            foreach (var timestamp in trigger.Timestamps)
            {
                timeline.RemoveEffectPoint(timestamp);
            }
        }

        public void LoadData(BeatmapEditorData data)
        {
            BiDictionary<int, EffectTimestamp> effectIds = timeline.GetEffectPointIdDictonary();
            
            foreach (var dataEffect in data.EffectTriggers)
            {
                Vector3 pos = new(0, dataEffect.Position.y, dataEffect.Position.x);
                GameObject gameObj = Instantiate(effectTriggerPrefab.gameObject, pos, Quaternion.identity);
                gameObj.transform.parent = transform;

                EffectTrigger trigger = gameObj.GetComponent<EffectTrigger>();
                trigger.TimingFunction = dataEffect.TimingFunction;
                trigger.BeginTime = dataEffect.BeginTime;
                trigger.EndTime = dataEffect.EndTime;
                trigger.Object = objectManager.GetObjectById(dataEffect.ObjectId);
                trigger.Timeline = timeline;
                trigger.Timestamps.AddRange(from id in dataEffect.Effects select effectIds.KeyMap[id]);

                _effectTriggers.Add(trigger);
            }
        }

        public void SaveData(BeatmapEditorData data)
        {
            BiDictionary<int, EffectTimestamp> effectIds = timeline.GetEffectPointIdDictonary();

            data.EffectTriggers.AddRange(from trigger in _effectTriggers
                select new EffectTriggerData
                {
                    Position = new Vector2(trigger.BeginPosition.y, trigger.BeginPosition.z),
                    BeginTime = trigger.BeginTime,
                    EndTime = trigger.EndTime,
                    TimingFunction = trigger.TimingFunction,
                    ObjectId = objectManager.GetIdByObject(trigger.Object),
                    Effects = (from timestamp in trigger.Timestamps select effectIds.ValueMap[timestamp]).ToList()
                });
        }
    }
}