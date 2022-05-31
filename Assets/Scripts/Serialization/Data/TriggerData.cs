using System;
using System.Collections.Generic;
using VisualEffect.Function;

namespace Serialization.Data
{
    [Serializable]
    public class TriggerData
    {
        // Идентификатор триггера
        public long Id;
        
        // Идентификатор объекта, на который влияет триггер (переход)
        public long TargetId;
        
        // Функция плавности триггера (перехода)
        public ITimingFunction Function;
        
        // Время, когда триггер (переход) сработает
        public TimeData Timestamp;
        
        // Длительность триггера (перехода)
        public TimeData Duration;
        
        // Список свойств, которые нужно изменить во время перехода
        public List<TransitionData> Transitions = new();
    }
}