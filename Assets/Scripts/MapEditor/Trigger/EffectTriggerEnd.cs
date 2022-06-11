using MapEditor.Timestamp;
using MapEditor.Utils;
using UnityEngine;

namespace MapEditor.Trigger
{
    public class EffectTriggerEnd : MonoBehaviour, IMovable
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
            Vector3 begin = _effectTrigger.BeginPosition;
            if (pos.x >= begin.x)
            {
                _effectTrigger.EndPosition = pos;
                return true;
            }

            return false;
        }

        public bool OnEndMove()
        {
            if (!OnMove())
            {
                _effectTrigger.EndPosition = _beginMovePosition;
                return false;
            }

            Vector3 position = transform.position;
            Vector3 begin = _effectTrigger.BeginPosition;
            _effectTrigger.BeginPosition = new Vector3(begin.x, position.y, position.z);
            
            Timeline.RemoveEffectPoint(Timestamp);

            ITime endTime = ITime.OfSecond(Timeline.GetSecondByPosition(position.x));
            
            _effectTrigger.Timestamp = Timeline.AddEffectPoint(
                Timestamp.BeginTime,
                endTime,
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