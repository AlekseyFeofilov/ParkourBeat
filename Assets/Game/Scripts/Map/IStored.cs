using Game.Scripts.Serialization.Data;

namespace Game.Scripts.Map
{
    public interface IStored
    {
        public void SaveData(BeatmapData data);

        public void LoadData(BeatmapData data);
    }
}