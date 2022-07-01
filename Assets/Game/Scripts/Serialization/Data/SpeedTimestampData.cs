using System;
using Game.Scripts.Map.Timestamp;

namespace Game.Scripts.Serialization.Data
{
    [Serializable]
    public class SpeedTimestampData
    {
        public MapTime Time;
        public double Second;
        public double Speed;
        public double Position;
    }
}