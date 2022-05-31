using System.IO;
using System.Text;
using Newtonsoft.Json;
using Serialization.Converter;
using Serialization.Data;
using UnityEngine;

public class SampleSave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
        trigger1.Timestamp = "8b";
        trigger1.Duration = ".2s";
        trigger1.Function = "ease";

        transition1.Type = "Position";
        transition1.Data = new Vector3(1f, 2f, 2.4f);
           
        transition2.Type = "Color";
        transition2.Data = new Color(1f, 0.5f, 0.1f, 1f);
        
        JsonSerializer jsonSerializer = new();
        jsonSerializer.Converters.Add(new Vector3Converter());
        jsonSerializer.Converters.Add(new ColorConverter());
        jsonSerializer.Converters.Add(new TransitionConverter());

        string json = Serialize(beatmap, jsonSerializer);
        BeatmapData obj = Deserialize<BeatmapData>(json, jsonSerializer);
        json = Serialize(beatmap, jsonSerializer);
        Debug.Log(json);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private string Serialize(
        object value,
        JsonSerializer jsonSerializer)
    {
        StringWriter stringWriter = new(new StringBuilder(256));
        using (JsonTextWriter jsonTextWriter = new(stringWriter))
        {
            jsonTextWriter.Formatting = jsonSerializer.Formatting;
            jsonSerializer.Serialize(jsonTextWriter, value);
        }
        return stringWriter.ToString();
    }
    
    private T Deserialize<T>(
        string json,
        JsonSerializer jsonSerializer)
    {
        using JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(json));
        return (T) jsonSerializer.Deserialize(jsonTextReader, typeof (T));
    }
}
