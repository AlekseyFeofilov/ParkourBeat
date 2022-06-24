using System;
using MapEditor.Timestamp;
using VisualEffect.Function;

namespace Serialization.Data
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