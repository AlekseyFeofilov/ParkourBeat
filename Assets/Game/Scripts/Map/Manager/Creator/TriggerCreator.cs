using System.Collections;
using Game.Scripts.Engine.Manager;
using Game.Scripts.Map.Timestamp;
using Game.Scripts.Map.VisualEffect.Function;
using Game.Scripts.MapEditor.Trigger;
using UnityEngine;

namespace Game.Scripts.Map.Manager.Creator
{
    public class TriggerCreator : MonoBehaviour
    {
        [SerializeField] private Timeline.Timeline timeline;
        [SerializeField] private TriggerManager triggerManager;
        [SerializeField] private SelectManager selectManager;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.T)) CreateTriggerFromSelectedObject();
        }
        
        public void CreateTriggerFromSelectedObject()
        {
            if (Camera.main is null) return;

            Vector3 position = Camera.main.transform.position;
            position = new Vector3(position.x, 0, position.z);
            MapTime time = MapTime.OfSecond(timeline.GetSecondByPosition(position.x));
            MapTime duration = MapTime.OfSecond(1);
            ITimingFunction function = ITimingFunction.Linear;
            EffectTrigger trigger = triggerManager.CreateEffectTrigger(position, time, duration, function);
            StartCoroutine(SelectTriggerDelayed(trigger));
        }

        private IEnumerator SelectTriggerDelayed(EffectTrigger trigger)
        {
            yield return 0;
            selectManager.Deselect();
            selectManager.Select(trigger.transform.GetChild(0).gameObject);
        }
    }
}