using System.IO;
using System.Text;
using Newtonsoft.Json;
using Serialization;
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

        beatmap.Elements.Add(element1);
        beatmap.Triggers.Add(trigger1);
        trigger1.Transitions.Add(transition1);
        
        element1.Id = 0;
        trigger1.Id = 0;
        trigger1.TargetId = 0;
        
        element1.Type = "test";
        trigger1.Timestamp = "8b";
        trigger1.Duration = ".2s";
        trigger1.Function = "ease";

        transition1.Type = "Position";
        transition1.Data = new Vector3(1f, 2f, 2.4f);
            
        JsonSerializer jsonSerializer = new();
        jsonSerializer.Converters.Add(new TransitionJsonConverter());

        string json = Serialize(beatmap, jsonSerializer);
        Debug.Log(json);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private string Serialize(
        object? value,
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
}
