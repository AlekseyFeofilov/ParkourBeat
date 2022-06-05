using MapEditor.Timestamp;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Property;

namespace VisualEffect.Point
{
    public class VisualEffectPoint
    {
        // Время, когда нужно применить этот эффект
        public ITime Time;
        
        // Длительность эффекта
        public ITime Duration;
        
        // Функция плавности
        public ITimingFunction TimingFunction;
        
        // Свойство, к которому применяется эффект
        public IVisualProperty Property;

        // От какого состояния начинается переход
        public object FromState;
        
        // К какому состоянию нужно привести свойство
        public object ToState;

        // Время конца эффекта
        public ITime EndTime => Time + Duration;

        public VisualEffectPoint()
        {
        }

        public VisualEffectPoint(
            ITime time, 
            ITime duration, 
            ITimingFunction timingFunction, 
            IVisualProperty property, 
            object toState)
        {
            Time = time;
            Duration = duration;
            TimingFunction = timingFunction;
            Property = property;
            ToState = toState;
        }

        public void Update(double second, ITimeConverter converter)
        {
            double begin = Time.ToSecond(converter);
            double end = EndTime.ToSecond(converter);
            double duration = end - begin;
            float time = (float) ((second - begin) / duration);
            float multiplier = Mathf.Max(0, Mathf.Min(1, TimingFunction.Get(time)));
            Property.Update(multiplier, FromState, ToState);
        }
    }
}