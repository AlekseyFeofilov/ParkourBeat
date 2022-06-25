using System.Collections.Generic;
using System.Linq;
using DataStructures.BiDictionary;
using MapEditor;
using MapEditor.Timestamp;
using MapEditor.Trigger;
using Serialization.Data;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Object;
using VisualEffect.Property;

namespace Beatmap.Trigger
{
    public class TriggerManager : MonoBehaviour
    {
        [SerializeField] private Timeline timeline;
        [SerializeField] private EffectTrigger effectTriggerPrefab;

        private readonly List<EffectTrigger> _effectTriggers = new();

        public EffectTrigger CreateEffectTrigger(
            MapTime beginTime,
            MapTime duration,
            ITimingFunction function,
            IVisualProperty property,
            object state)
        {
            EffectTrigger trigger = CreateEffectTrigger(beginTime, duration, function);
            trigger.AddProperty(property, state);
            return trigger;
        }

        public EffectTrigger CreateEffectTrigger(
            MapTime beginTime,
            MapTime duration,
            ITimingFunction function)
        {
            GameObject gameObj = Instantiate(effectTriggerPrefab.gameObject, Vector3.zero * 1, Quaternion.identity);
            gameObj.transform.parent = transform;

            TimeUnit endUnit = beginTime.Unit == duration.Unit ? duration.Unit : TimeUnit.Second;
            double endValue = beginTime.Unit == duration.Unit 
                ? beginTime.Value + duration.Value 
                : beginTime.ToSecond(timeline) + duration.ToSecond(timeline);
            MapTime endTime = new MapTime(endUnit, endValue);

            EffectTrigger trigger = gameObj.GetComponent<EffectTrigger>();
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
            foreach (var timestamp in trigger.Timestamps.Values)
            {
                timeline.RemoveEffectPoint(timestamp);
            }
        }

        public void RemoveObject(MonoObject monoObject)
        {
            foreach (var property in monoObject.Properties.ValueMap.Keys)
            {
                foreach (var trigger in _effectTriggers.Where(trigger => trigger.Timestamps.ContainsKey(property)))
                {
                    trigger.Timestamps.Remove(property);
                }
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
                trigger.Timeline = timeline;

                foreach (EffectTimestamp effect in dataEffect.Effects.Select(id => effectIds.KeyMap[id]))
                {
                    trigger.Timestamps[effect.Property] = effect;
                }

                _effectTriggers.Add(trigger);
            }
        }

        public void SaveData(BeatmapEditorData data)
        {
            BiDictionary<int, EffectTimestamp> effectIds = timeline.GetEffectPointIdDictonary();

            data.EffectTriggers.AddRange(from trigger in _effectTriggers
                select new EffectTriggerData
                {
                    Position = new Vector2(trigger.BeginPosition.z, trigger.BeginPosition.y),
                    BeginTime = trigger.BeginTime,
                    EndTime = trigger.EndTime,
                    TimingFunction = trigger.TimingFunction,
                    Effects = (from timestamp in trigger.Timestamps.Values select effectIds.ValueMap[timestamp]).ToList()
                });
        }
    }
}