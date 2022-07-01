using System;
using Game.Scripts.Map.Timestamp;
using Game.Scripts.Map.VisualEffect.Function;

namespace Game.Scripts.Serialization.Data
{
    [Serializable]
    public class EffectTimestampData
    {
        public int Index;
        public MapTime BeginTime;
        public MapTime EndTime;
        public ITimingFunction TimingFunction;
        public VisualPropertyId Property;
        public ValueData FromState;
        public ValueData ToState;
    }
}