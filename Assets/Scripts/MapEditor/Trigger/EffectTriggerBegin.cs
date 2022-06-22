using System.Collections.Generic;
using MapEditor.Timestamp;
using MapEditor.Utils;
using UnityEngine;

namespace MapEditor.Trigger
{
    public class EffectTriggerBegin : MonoBehaviour, IMovable
    {
        private const float Epsilon = 0.001f;

        private EffectTrigger _effectTrigger;
        private Vector3 _beginMovePosition;

        private ITimeline Timeline => _effectTrigger.Timeline;
        private List<EffectTimestamp> Timestamps => _effectTrigger.Timestamps;
        
        private void Start()
        {
            _effectTrigger = transform.parent.GetComponent<EffectTrigger>();
        }

        public bool OnBeginMove()
        {
            _beginMovePosition = transform.position;
            return true;
        }
        
        public bool OnMove()
        {
            Vector3 pos = transform.position;
            Vector3 end = _effectTrigger.EndPosition;
            if (pos.x <= end.x)
            {
                _effectTrigger.BeginPosition = pos;
                return true;
            }

            return false;
        }
        
        public bool OnEndMove()
        {
            if (!OnMove())
            {
                _effectTrigger.BeginPosition = _beginMovePosition;
                return false;
            }
            
            Vector3 position = transform.position;
            Vector3 end = _effectTrigger.EndPosition;
            _effectTrigger.EndPosition = new Vector3(end.x, position.y, position.z);
            
            MapTime beginTime = MapTime.OfSecond(Timeline.GetSecondByPosition(position.x));
            
            List<EffectTimestamp> toAdd = new();
            foreach (var timestamp in Timestamps)
            {
                Timeline.RemoveEffectPoint(timestamp);
                
                toAdd.Add(Timeline.AddEffectPoint(
                    beginTime,
                    timestamp.EndTime,
                    timestamp.TimingFunction,
                    timestamp.Property,
                    timestamp.ToState
                ));
            }
            
            _effectTrigger.Timestamps.Clear();
            _effectTrigger.Timestamps.AddRange(toAdd);
            _effectTrigger.BeginTime = beginTime;

            // Обязательно нужно, чтобы не было багов из-за прошлых значений
            Timeline.ResetMove();
            return true;
        }
    }
}