using System;
using System.Collections.Generic;

namespace Game.Scripts.Serialization.Data
{
    [Serializable]
    public class ObjectData
    {
        public long Id;
        public string Type;
        public List<VisualPropertyData> Properties = new();
    }
}