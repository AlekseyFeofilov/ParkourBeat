using System;

namespace Game.Scripts.Map.Meta
{
    [Serializable]
    public class BeatmapMeta
    {
        public string displayName;
        public string description;
        public string author;
        public long publicationDate;
        public long updateDate;
        public bool yoursMap;
        public bool defaultMap;
        public string assessment;
        public string difficulty;
    }
}