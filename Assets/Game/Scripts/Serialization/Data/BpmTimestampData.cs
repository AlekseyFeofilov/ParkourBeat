using System;

namespace Game.Scripts.Serialization.Data
{
    [Serializable]
    public class BpmTimestampData
    {
        public double Second;
        public double Bpm;
        public double Beat;
    }
}