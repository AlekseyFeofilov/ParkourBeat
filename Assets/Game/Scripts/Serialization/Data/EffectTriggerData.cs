using System.Collections.Generic;
using Game.Scripts.Map.Timestamp;
using Game.Scripts.Map.VisualEffect.Function;
using UnityEngine;

namespace Game.Scripts.Serialization.Data
{
    public class EffectTriggerData
    {
        public Vector2 Position;
        public MapTime BeginTime;
        public MapTime EndTime;
        public ITimingFunction TimingFunction;
        public List<int> Effects;
    }
}