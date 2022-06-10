using System;
using System.Diagnostics.CodeAnalysis;
using MapEditor.Timestamp;
using MapEditor.Utils;
using UnityEngine;

namespace MapEditor.Trigger
{
    public class EffectTrigger : MonoBehaviour, IMovable
    {
        private const float Epsilon = 0.001f;

        public ITimeline Timeline;

        public EffectTimestamp Timestamp;
        
        private Vector3 _beginMovePosition;
        
        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        private void Start()
        {
            double second = Timestamp.BeginTime.ToSecond(Timeline);
            float x = (float) Timeline.GetPositionBySecond(second);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);

            gameObject.layer = LayerMask.NameToLayer("Selectable Mask");
            OutlinedObject outlined = gameObject.AddComponent<OutlinedObject>();
            outlined.OutlineWidth = 0f;
        }

        public void OnBeginMove()
        {
            _beginMovePosition = transform.position;
        }
        
        public void OnEndMove()
        {
            if (Math.Abs(_beginMovePosition.x - transform.position.x) < Epsilon)
            {
                return;
            }
            
            Timeline.RemoveEffectPoint(Timestamp);

            ITime time = ITime.OfSecond(Timeline.GetSecondByPosition(transform.position.x));
            
            Timestamp = Timeline.AddEffectPoint(
                time,
                Timestamp.Duration,
                Timestamp.TimingFunction,
                Timestamp.Property,
                Timestamp.ToState
                );

            // Обязательно нужно, чтобы не было багов из-за прошлых значений
            Timeline.ResetMove();
        }
        
        public EffectTrigger SetX(float x)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(x, pos.y, pos.z);
            return this;
        }
        
        public EffectTrigger SetY(float y)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, y, pos.z);
            return this;
        }
        
        public EffectTrigger SetZ(float z)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, z);
            return this;
        }
        
        public EffectTrigger AddX(float x)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x + x, pos.y, pos.z);
            return this;
        }
        
        public EffectTrigger AddY(float y)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y + y, pos.z);
            return this;
        }
        
        public EffectTrigger AddZ(float z)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, pos.y, pos.z + z);
            return this;
        }
    }
}