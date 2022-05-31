using System;

namespace Serialization.Data
{
    [Serializable]
    public class TransitionData
    {
        public TransitionData()
        {
        }

        public TransitionData(string type, object data)
        {
            Type = type;
            Data = data;
        }

        public string Type;
        public object Data;
    }
}