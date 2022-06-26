using Game.Scripts.Engine.Manager;
using Game.Scripts.Map.VisualEffect.Object;
using Game.Scripts.MapEditor.Trigger;
using UnityEngine;

namespace Game.Scripts.MapEditor
{
    public class BeatmapModeManager : MonoBehaviour
    {
        [SerializeField] private Map.Timeline.Timeline timeline;
        [SerializeField] private ToolManager toolManager;
        
        // TODO сделано по тупому с MainSelect.Selected
        private void Update()
        {
            // Превью режим
            if (BeatmapEditorContext.Mode == BeatmapEditorContext.ToolMode.Global &&
                (SelectManager.Selected.Count == 0 ||
                 !SelectManager.Selected[0].GetComponent<MonoObject>()) &&
                !Input.GetKey(KeyCode.LeftAlt))
            {
                BeatmapEditorContext.Reset();
                return;
            }
            
            // Глобальный режим
            if (BeatmapEditorContext.Mode == BeatmapEditorContext.ToolMode.Preview &&
                (SelectManager.Selected.Count > 0 &&
                SelectManager.Selected[0].GetComponent<MonoObject>() || 
                Input.GetKey(KeyCode.LeftAlt)))
            {
                BeatmapEditorContext.Trigger = null;
                BeatmapEditorContext.Mode = BeatmapEditorContext.ToolMode.Global;
                timeline.Move(0);
                return;
            }

            if (!Input.GetKeyDown(KeyCode.F) ||
                toolManager.Activated &&
                toolManager.ToolMode == ToolManager.Mode.ColorTool) return;
            ResetTrigger();

            // Режим триггера
            if (SelectManager.Selected.Count > 0 &&
                SelectManager.Selected[0].gameObject.TryGetComponent(out IEffectTriggerPart part) &&
                BeatmapEditorContext.Trigger != part.Parent)
            {
                BeatmapEditorContext.Trigger = part.Parent;
                BeatmapEditorContext.Mode = BeatmapEditorContext.ToolMode.Trigger;
                BeatmapEditorContext.UpdateTrigger(timeline);
            }
            // Глобальный режим
            else if (SelectManager.Selected.Count > 0 &&
                     SelectManager.Selected[0].GetComponent<MonoObject>())
            {
                BeatmapEditorContext.Trigger = null;
                BeatmapEditorContext.Mode = BeatmapEditorContext.ToolMode.Global;
                timeline.Move(0);
            }
            // Превью режим
            else
            {
                BeatmapEditorContext.Reset();
            }
        }

        private void ResetTrigger()
        {
            if (BeatmapEditorContext.Trigger == null) return;
            
            foreach (var property in BeatmapEditorContext.Trigger.Timestamps.Keys)
            {
                property.Apply(property.GetDefault());   
            }

            BeatmapEditorContext.Trigger.Selected = false;
        }
    }
}