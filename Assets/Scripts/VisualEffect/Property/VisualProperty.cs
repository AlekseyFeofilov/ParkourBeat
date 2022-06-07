using System;
using System.Collections.Generic;
using MapEditor.Trigger;

namespace VisualEffect.Property
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VisualPropertyAttribute : Attribute
    {
        
    }

    public interface IVisualProperty
    {
        List<EffectTrigger> Points { get; }

        void Apply(object state);
        
        void Update(float multiplier, object from, object to);

        object GetDefault();
    }

    public abstract class AbstractVisualProperty<T> : IVisualProperty
    {
        private List<EffectTrigger> _points = new();

        public List<EffectTrigger> Points => _points;

        public abstract T Default { get; set; }
        
        protected abstract void Apply(T state);

        protected abstract void Update(float multiplier, T from, T to);

        public void Apply(object state)
        {
            if (state is not T stateCasted) return;
            Apply(stateCasted);
        }
        
        public void Update(float multiplier, object from, object to)
        {
            if (from is not T fromCasted || to is not T toCasted) return;
            Update(multiplier, fromCasted, toCasted);
        }
        
        public object GetDefault()
        {
            return Default;
        }
    }
}