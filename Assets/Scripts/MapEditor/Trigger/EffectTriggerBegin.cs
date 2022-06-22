using System.Collections.Generic;
using MapEditor.ChangeableInterfaces;
using MapEditor.Timestamp;
using UnityEngine;
using VisualEffect.Property;

namespace MapEditor.Trigger
{
    public class EffectTriggerBegin : MonoBehaviour, IMovable, IEffectTriggerPart
    {
        private const float Epsilon = 0.001f;

        private Vector3 _beginMovePosition;

        private ITimeline Timeline => Parent.Timeline;
        private Dictionary<IVisualProperty, EffectTimestamp> Timestamps => Parent.Timestamps;
        
        public EffectTrigger Parent { get; private set; }

        private void Start()
        {
            Parent = transform.parent.GetComponent<EffectTrigger>();
        }

        public bool OnBeginMove()
        {
            _beginMovePosition = transform.position;
            return true;
        }
        
        public bool OnMove(Vector3 movement)
        {
            return TryMove();
        }

        private bool TryMove()
        {
            Vector3 pos = transform.position;
            Vector3 end = Parent.EndPosition;
            if (pos.x <= end.x)
            {
                Parent.BeginPosition = pos;
                return true;
            }

            return false;
        }
        
        public bool OnEndMove()
        {
            if (!TryMove())
            {
                Parent.BeginPosition = _beginMovePosition;
                return false;
            }
            
            Vector3 position = transform.position;
            Vector3 end = Parent.EndPosition;
            Parent.EndPosition = new Vector3(end.x, position.y, position.z);
            
            MapTime beginTime = MapTime.OfSecond(Timeline.GetSecondByPosition(position.x));
            
            Dictionary<IVisualProperty, EffectTimestamp> toAdd = new();
            foreach (var (key, timestamp) in Timestamps)
            {
                Timeline.RemoveEffectPoint(timestamp);
                
                toAdd[key] = Timeline.AddEffectPoint(
                    beginTime,
                    timestamp.EndTime,
                    timestamp.TimingFunction,
                    timestamp.Property,
                    timestamp.ToState
                );
            }
            Parent.Timestamps.Clear();

            foreach (var (key, value) in toAdd)
            {
                Parent.Timestamps[key] = value;   
            }
            Parent.BeginTime = beginTime;
            
            // Если режим триггера
            if (BeatmapEditorContext.Mode == BeatmapEditorContext.ToolMode.Trigger)
            {
                BeatmapEditorContext.UpdateTrigger(Timeline);
            }
            return true;
        }
    }
}