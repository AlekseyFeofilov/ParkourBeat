using System;
using Game.Scripts.Map.Timeline;
using Game.Scripts.Map.Timestamp;
using Game.Scripts.Map.VisualEffect.Property;
using Game.Scripts.MapEditor.Trigger;
using Libraries.QuickOutline.Scripts;
using UnityEngine;

namespace Game.Scripts.MapEditor
{
    public class BeatmapEditorContext
    {
        public static ToolMode Mode = ToolMode.Preview;
        public static EffectTrigger Trigger;

        public static Map.Timeline.Timeline LazyTimeline;

        public static Map.Timeline.Timeline GetTimeline()
        {
            if (LazyTimeline == null)
            {
                LazyTimeline = GameObject.FindObjectOfType<Map.Timeline.Timeline>();
            }

            return LazyTimeline;
        }
        
        public static void SetPropertyValue<T>(AbstractVisualProperty<T> property, T value)
        {
            switch (Mode)
            {
                case ToolMode.Global:
                    GetTimeline().UpdateDefault(property, value);
                    break;
                case ToolMode.Trigger:
                    if (Trigger.Timestamps.TryGetValue(property, out EffectTimestamp effect))
                    {
                        GetTimeline().UpdateEffect(effect, value);
                    }
                    else
                    {
                        Trigger.AddProperty(property, value);
                    }
                    break;
                case ToolMode.Preview:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void UpdateTrigger(ITimeline timeline)
        {
            if (Trigger is null) return;
            
            timeline.Move(Trigger.BeginTime.ToSecond(timeline));
                        
            foreach (var effect in Trigger.Timestamps.Values)
            {
                effect.Property.Apply(effect.ToState);   
            }
                        
            if (Trigger.TryGetComponent(out OutlinedObject outlined))
            {
                outlined.OutlineMode = OutlinedObject.Mode.OutlineAll;
            }
        }

        public static void Reset()
        {
            Mode = ToolMode.Preview;
            Trigger = null;
        }
        
        public enum ToolMode
        {
            Preview,
            Global,
            Trigger
        }
    }
}