using System;
using System.Collections.Generic;

namespace Serialization.Data
{
    [Serializable]
    public class BeatmapData
    {
        public int GameVersion = 1;

        public long UpdateDate;
        
        // Список элементов на карте
        public List<ObjectData> Objects = new();
        
        // Список триггеров BPM на карте
        public List<BpmTimestampData> Bpm = new();
        
        // Список триггеров скорости на карте
        public List<SpeedTimestampData> Speed = new();
        
        // Список триггеров эффектов на карте
        public List<EffectTimestampData> BeginSortedEffects = new();

        public List<int> EndSortedEffects = new();
    }
}