using System;

namespace VisualEffect.Property
{
    [AttributeUsage(AttributeTargets.Field)]
    public class VisualPropertyAttribute : Attribute
    {
        
    }

    public interface IVisualProperty
    {
        void Apply(object state);

        object Calculate(float multiplier, object from, object to);

        object GetDefault();
    }

    public abstract class AbstractVisualProperty<T> : IVisualProperty
    {
        public abstract T Default { get; set; }
        
        protected abstract void Apply(T state);

        protected abstract T Calculate(float multiplier, T from, T to);

        public object Calculate(float multiplier, object from, object to)
        {
            if (from is not T fromCasted || to is not T toCasted) 
                throw new InvalidCastException();
            return Calculate(multiplier, fromCasted, toCasted);
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
    }
}