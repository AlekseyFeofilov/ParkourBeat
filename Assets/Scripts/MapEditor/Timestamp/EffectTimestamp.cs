using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Property;

namespace MapEditor.Timestamp
{
    public class EffectTimestamp
    {
        // Время, когда эффект начинается
        public readonly MapTime BeginTime;
        
        // Время, когда эффект заканчивается
        public readonly MapTime EndTime;
        
        // Функция плавности
        public ITimingFunction TimingFunction;
        
        // Свойство, к которому применяется эффект
        public IVisualProperty Property;

        // От какого состояния начинается переход
        public object FromState;
        
        // К какому состоянию нужно привести свойство
        public object ToState;

        public EffectTimestamp(MapTime beginTime, MapTime endTime)
        {
            BeginTime = beginTime;
            EndTime = endTime;
        }

        public EffectTimestamp(MapTime beginTime, MapTime endTime, ITimingFunction timingFunction, IVisualProperty property, object toState)
        {
            BeginTime = beginTime;
            EndTime = endTime;
            TimingFunction = timingFunction;
            Property = property;
            ToState = toState;
        }
        
        public object CalculateState(double second, ITimeConverter converter)
        {
            double begin = BeginTime.ToSecond(converter);
            double end = EndTime.ToSecond(converter);
            double duration = end - begin;

            if (duration == 0) return ToState;
            
            float time = Mathf.Max(0, Mathf.Min(1, (float) ((second - begin) / duration)));
            float multiplier = Mathf.Max(0, Mathf.Min(1, TimingFunction.Get(time)));
            return Property.Calculate(multiplier, FromState, ToState);
        }

        public void UpdateEffect(double second, ITimeConverter converter)
        {
            object state = CalculateState(second, converter);
            Property.Apply(state);
        }
    }
}