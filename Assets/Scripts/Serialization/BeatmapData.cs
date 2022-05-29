using System;
using System.Collections.Generic;

namespace Serialization
{
    [Serializable]
    public class BeatmapData
    {
        public List<ElementData> Elements = new();
        public List<TriggerData> Triggers = new();
    }
}