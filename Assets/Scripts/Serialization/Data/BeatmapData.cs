using System;
using System.Collections.Generic;

namespace Serialization.Data
{
    [Serializable]
    public class BeatmapData
    {
        // Список элементов на карте
        public List<ElementData> Elements = new();
        
        // Список триггеров на карте
        public List<TriggerData> Triggers = new();
    }
}