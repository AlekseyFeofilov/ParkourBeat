using MapEditor;
using MapEditor.Select;
using MapEditor.Timestamp;
using MapEditor.Trigger;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Object;

namespace Beatmap.Trigger
{
    public class TriggerCreator : MonoBehaviour
    {
        [SerializeField] private Timeline timeline;
        [SerializeField] private TriggerManager triggerManager;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.T)) CreateTriggerFromSelectedObject();
        }
        
        public void CreateTriggerFromSelectedObject()
        {
            if (Camera.main is null) return;
            if (MainSelect.SelectedObj is null) return;
            
            GameObject selected = MainSelect.SelectedObj.gameObject;
            if (!selected.TryGetComponent(out MonoObject monoObject)) return;
            
            double x = Camera.main.transform.position.x;
            MapTime time = MapTime.OfSecond(timeline.GetSecondByPosition(x));
            MapTime duration = MapTime.OfBeat(4);
            ITimingFunction function = ITimingFunction.Linear;
            triggerManager.CreateEffectTrigger(time, duration, function, monoObject);
        }
    }
}