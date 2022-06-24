using System;
using VisualEffect.Object;

namespace VisualEffect.Property
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VisualPropertyAttribute : Attribute
    {
        public string Id;
    }

    public interface IVisualProperty
    {
        public MonoObject Parent { get; set; }

        public void Apply(object state);

        public object Calculate(float multiplier, object from, object to);

        public object GetDefault();

        public void SetDefault(object def);
    }

    public abstract class AbstractVisualProperty<T> : IVisualProperty
    {
        private MonoObject _parent;
        
        public abstract T Default { get; set; }
        
        protected abstract void Apply(T state);

        protected abstract T Calculate(float multiplier, T from, T to);

        public object Calculate(float multiplier, object from, object to)
        {
            if (from is not T fromCasted || to is not T toCasted) 
                throw new InvalidCastException();
            return Calculate(multiplier, fromCasted, toCasted);
        }

        public MonoObject Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public void Apply(object state)
        {
            if (state is not T stateCasted)
                throw new InvalidCastException();
            Apply(stateCasted);
        }
        
        public object GetDefault()
        {
            return Default;
        }

        public void SetDefault(object def)
        {
            if (def is not T defCasted)
                throw new InvalidCastException();
            Default = defCasted;
        }
    }
}