namespace MenuMap
{
    public class BeatmapInfo
    {
        public readonly string Name;
        public readonly BeatmapMeta Meta;

        public BeatmapInfo(string name, BeatmapMeta meta)
        {
            Name = name;
            Meta = meta;
        }
    }
}