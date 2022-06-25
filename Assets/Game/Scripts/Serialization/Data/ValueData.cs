using System;

namespace Game.Scripts.Serialization.Data
{
    [Serializable]
    public class ValueData
    {
        public object Value;

        public ValueData(object value)
        {
            Value = value;
        }
    }
}