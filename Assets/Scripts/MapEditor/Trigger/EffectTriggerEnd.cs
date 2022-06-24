using System.Collections.Generic;
using MapEditor.ChangeableInterfaces;
using MapEditor.Select;
using MapEditor.Timestamp;
using MapEditor.Tools;
using UnityEngine;
using VisualEffect.Property;

namespace MapEditor.Trigger
{
    public class EffectTriggerEnd : MonoBehaviour, ISelectable, IMovable, IEffectTriggerPart
    {
        private const float Epsilon = 0.001f;

        private Vector3 _beginMovePosition;

        private ITimeline Timeline => Parent.Timeline;
        private Dictionary<IVisualProperty, EffectTimestamp> Timestamps => Parent.Timestamps;
        private MainTools _mainTools;
        
        public EffectTrigger Parent { get; private set; }

        private void Start()
        {
            _mainTools = FindObjectOfType<MainTools>();
            Parent = transform.parent.GetComponent<EffectTrigger>();
        }
        
        public void OnSelect(SelectEvent @event)
        {
            _mainTools.ToolMode = MainTools.Mode.MoveTool;
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
            Vector3 begin = Parent.BeginPosition;
            if (pos.x >= begin.x)
            {
                Parent.EndPosition = pos;
                return true;
            }

            return false;
        }

        public bool OnEndMove()
        {
            if (!TryMove())
            {
                Parent.EndPosition = _beginMovePosition;
                return false;
            }

            Vector3 position = transform.position;
            Vector3 begin = Parent.BeginPosition;
            Parent.BeginPosition = new Vector3(begin.x, position.y, position.z);

            MapTime endTime = MapTime.OfSecond(Timeline.GetSecondByPosition(position.x));

            Dictionary<IVisualProperty, EffectTimestamp> toAdd = new();
            foreach (var (key, timestamp) in Timestamps)
            {
                Timeline.RemoveEffectPoint(timestamp);
                
                toAdd[key] = Timeline.AddEffectPoint(
                    timestamp.BeginTime,
                    endTime,
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
            Parent.EndTime = endTime;
            return true;
        }
    }
}