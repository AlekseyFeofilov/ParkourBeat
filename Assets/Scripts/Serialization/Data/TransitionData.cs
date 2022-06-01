using System;

namespace Serialization.Data
{
    [Serializable]
    public class TransitionData
    {
        // Тип перехода (цвет, вектор)
        public string Type;
        
        // Данные
        public object Data;
        
        public TransitionData()
        {
        }

        public TransitionData(string type, object data)
        {
            Type = type;
            Data = data;
        }
    }
}