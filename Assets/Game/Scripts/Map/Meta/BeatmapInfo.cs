namespace Game.Scripts.Map.Meta
{
    public class BeatmapInfo
    {
        public readonly string Name;
        public readonly BeatmapMeta Meta;
        public readonly string Path;

        public BeatmapInfo(string name, BeatmapMeta meta, string path)
        {
            Name = name;
            Meta = meta;
            Path = path;
        }
    }
}