using System.Collections.Generic;
using Game.Scripts.Engine.Api.Event;
using Game.Scripts.Engine.Api.Listener;
using Game.Scripts.Engine.Manager;
using Game.Scripts.Map.Timeline;
using Game.Scripts.Map.Timestamp;
using Game.Scripts.Map.VisualEffect.Property;
using UnityEngine;

namespace Game.Scripts.MapEditor.Trigger
{
    public class EffectTriggerBegin : MonoBehaviour, ISelectable, IMovable, IEffectTriggerPart
    {
        private const float Epsilon = 0.001f;

        private Vector3 _beginMovePosition;

        private ITimeline Timeline => Parent.Timeline;
        private Dictionary<IVisualProperty, EffectTimestamp> Timestamps => Parent.Timestamps;
        private ToolManager _toolManager;
        
        public EffectTrigger Parent { get; private set; }

        private void Start()
        {
            _toolManager = FindObjectOfType<ToolManager>();
            Parent = transform.parent.GetComponent<EffectTrigger>();
        }

        public void OnSelect(SelectEvent @event)
        {
            _toolManager.ToolMode = ToolManager.Mode.MoveTool;
        }

        public bool OnBeginMove()
        {
            _beginMovePosition = transform.position;
            return true;
        }
        
        public bool OnMove(Vector3 movement)
        {
            TryMove();
            return true;
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