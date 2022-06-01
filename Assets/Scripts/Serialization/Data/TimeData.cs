using System;

namespace Serialization.Data
{
    [Serializable]
    public class TimeData
    {
        // Единица измерения времени
        public TimeUnit Unit;
        
        // Значение в единицах измерения
        public double Value;

        public TimeData()
        {
        }

        public TimeData(TimeUnit unit, double value)
        {
            Unit = unit;
            Value = value;
        }
    }

    public enum TimeUnit
    {
        // Секунды - абсолютная величина
        Second,
        
        // Биты - относительная величина, зависит от карты
        Beat
    }
}