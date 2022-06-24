using MapEditor;
using MapEditor.Timestamp;
using UnityEngine;
using VisualEffect.Function;

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

            double x = Camera.main.transform.position.x;
            MapTime time = MapTime.OfSecond(timeline.GetSecondByPosition(x));
            MapTime duration = MapTime.OfBeat(4);
            ITimingFunction function = ITimingFunction.Linear;
            triggerManager.CreateEffectTrigger(time, duration, function);
        }
    }
}