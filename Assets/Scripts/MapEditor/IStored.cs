using Serialization.Data;

namespace MapEditor
{
    public interface IStored
    {
        public void SaveData(BeatmapData data);

        public void LoadData(BeatmapData data);
    }
}