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
        private EffectTimestamp Timestamp => _effectTrigger.Timestamp;
        
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
            
            Timeline.RemoveEffectPoint(Timestamp);

            ITime beginTime = ITime.OfSecond(Timeline.GetSecondByPosition(position.x));
            
            _effectTrigger.Timestamp = Timeline.AddEffectPoint(
                beginTime,
                Timestamp.EndTime,
                Timestamp.TimingFunction,
                Timestamp.Property,
                Timestamp.ToState
            );

            // Обязательно нужно, чтобы не было багов из-за прошлых значений
            Timeline.ResetMove();
            return true;
        }
    }
}