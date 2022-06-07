using MapEditor.Timestamp;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Property;

namespace MapEditor.Trigger
{
    public class EffectTrigger : MonoBehaviour
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

        public void UpdateEffect(double second, ITimeConverter converter)
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