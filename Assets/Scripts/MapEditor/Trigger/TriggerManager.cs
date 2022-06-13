using MapEditor.Timestamp;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Property;

namespace MapEditor.Trigger
{
    public class TriggerManager : MonoBehaviour
    {
        [SerializeField] private Timeline timeline;
        [SerializeField] private EffectTrigger effectTriggerPrefab;
        
        public EffectTrigger CreateEffectTrigger(
            MapTime beginTime,
            MapTime duration,
            ITimingFunction function,
            IVisualProperty property,
            object state)
        {
            GameObject gameObj = Instantiate(effectTriggerPrefab.gameObject, Vector3.zero * 1, Quaternion.identity);
            gameObj.transform.parent = transform;

            TimeUnit endUnit = beginTime.Unit == duration.Unit ? duration.Unit : TimeUnit.Second;
            double endValue = beginTime.Unit == duration.Unit 
                ? beginTime.Value + duration.Value 
                : beginTime.ToSecond(timeline) + duration.ToSecond(timeline);
            MapTime endTime = new MapTime(endUnit, endValue);
                
            EffectTimestamp timestamp = 
                timeline.AddEffectPoint(beginTime, endTime, function, property, state);
            
            EffectTrigger trigger = gameObj.GetComponent<EffectTrigger>();
            trigger.Timeline = timeline;
            trigger.Timestamp = timestamp;
                
            return trigger;
        }
    }
}