using System;
using MapEditor.Timestamp;

namespace Serialization.Data
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