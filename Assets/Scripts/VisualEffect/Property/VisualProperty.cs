using System;
using UnityEngine;

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
    
    public interface IVisualProperty<T> : IVisualUpdatable
    {
        T Value { get; set; }

        void Transition(T value, float duration);
    }
}