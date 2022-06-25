using Game.Scripts.Map.Timestamp;
using Game.Scripts.Map.VisualEffect.Function;
using UnityEngine;

namespace Game.Scripts.Map.Manager.Creator
{
    public class TriggerCreator : MonoBehaviour
    {
        [SerializeField] private Timeline.Timeline timeline;
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