using System;
using UnityEngine;
using VisualEffect.Function;

namespace VisualEffect.Property
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VisualPropertyAttribute : Attribute
    {
        
    }

    public interface IVisualUpdatable
    {
        void Update();
    }

    public abstract class AbstractVisualProperty<T> : IVisualUpdatable
    {
        private ITimingFunction _timingFunction;
        private float _initialTime;
        private float _targetTime;
        protected T Initial;
        protected T Target;
        
        public abstract T Value { get; set; }

        protected abstract void OnUpdate(float multiplier);

        public void Update()
        {
            if (_timingFunction == null) return;
            
            float time = (Time.time - _initialTime) / (_targetTime - _initialTime);
            float multiplier = _timingFunction.Get(time);
                
            if (time >= 1) _timingFunction = null;

            OnUpdate(Math.Max(0, Math.Min(1, multiplier)));
        }
        
        public void BeginTransition(T value, float duration, ITimingFunction timingFunction)
        {
            if (duration == 0)
            {
                Value = value;
                return;
            }
            _timingFunction = timingFunction;
            Initial = Value;
            _initialTime = Time.time;
            Target = value;
            _targetTime = _initialTime + duration;
        }
    }
}