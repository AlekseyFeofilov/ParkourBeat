using System.Collections.Generic;
using MapEditor.Timestamp;
using UnityEngine;
using VisualEffect.Function;

namespace Serialization.Data
{
    public class EffectTriggerData
    {
        public Vector2 Position;
        public MapTime BeginTime;
        public MapTime EndTime;
        public ITimingFunction TimingFunction;
        public long ObjectId;
        public List<int> Effects;
    }
}