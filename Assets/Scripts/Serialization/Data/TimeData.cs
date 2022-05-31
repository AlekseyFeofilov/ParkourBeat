using System;

namespace Serialization.Data
{
    [Serializable]
    public class TimeData
    {
        public TimeUnit Unit;
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
        Second, 
        Beat
    }
}