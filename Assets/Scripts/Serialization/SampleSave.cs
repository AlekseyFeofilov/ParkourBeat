using Serialization.Converter;
using Serialization.Data;
using UnityEngine;
using VisualEffect.Function;

namespace Serialization
{
    public class SampleSave : MonoBehaviour
    {
        // Пример сохранения и чтения данных
        private void Start()
        {
            // Подготавливаем beatmap на сериализацию
            BeatmapData beatmap = new();
            ElementData element1 = new();
            TriggerData trigger1 = new();
            TransitionData transition1 = new();
            TransitionData transition2 = new();

            beatmap.Elements.Add(element1);
            beatmap.Triggers.Add(trigger1);
            trigger1.Transitions.Add(transition1);
            trigger1.Transitions.Add(transition2);
        
            element1.Id = 0;
            element1.Type = "test";
        
            trigger1.Id = 0;
            trigger1.TargetId = 0;
            trigger1.Timestamp = new TimeData(TimeUnit.Beat, 8);
            trigger1.Duration = new TimeData(TimeUnit.Second, .2);
            trigger1.Function = ITimingFunction.Ease;

            transition1.Type = "Position";
            transition1.Data = new Vector3(1f, 2f, 2.4f);
           
            transition2.Type = "Color";
            transition2.Data = new Color(1f, 0.5f, 0.1f, 1f);
        
            // Подготавливаем json manager
            JsonManager jsonManager = new();
            jsonManager.AddConverter(new Vector3Converter());
            jsonManager.AddConverter(new ColorConverter());
            jsonManager.AddConverter(new TimeConverter());
            jsonManager.AddConverter(new TimingFunctionConverter());
            jsonManager.AddConverter(new TransitionConverter());

            // Сериализуем, десериализуем и потом опять сериализуем
            string json = jsonManager.Serialize(beatmap);
            beatmap = jsonManager.Deserialize<BeatmapData>(json);
            json = jsonManager.Serialize(beatmap);
            
            // Выводим
            Debug.Log(json);
        }
    }
}
