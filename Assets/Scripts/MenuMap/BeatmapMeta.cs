using System;

namespace MenuMap
{
    [Serializable]
    public class BeatmapMeta
    {
        public string displayName;
        public string description;
        public string author;
        public long publicationDate;
        public long updateDate;
        public bool yourMap;
        public bool defaultMap;

        private void ShowInfoAboutLevel()
        {
        }
    }
}