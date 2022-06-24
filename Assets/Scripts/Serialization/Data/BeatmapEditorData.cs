using System.Collections.Generic;

namespace Serialization.Data
{
    public class BeatmapEditorData
    {
        public int GameVersion = 1;
        
        // Список триггеров эффектов на карте
        public List<EffectTriggerData> EffectTriggers = new();
    }
}