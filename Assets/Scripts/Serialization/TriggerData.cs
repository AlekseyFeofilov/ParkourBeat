using System;
using System.Collections.Generic;

namespace Serialization
{
    [Serializable]
    public class TriggerData
    {
        public long Id;
        public long TargetId;
        public string Function;
        public string Timestamp;
        public string Duration;
        public List<TransitionData> Transitions = new();
    }
}