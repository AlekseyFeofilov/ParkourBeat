using MapEditor.Select;
using MapEditor.Trigger;
using UnityEngine;
using VisualEffect.Object;

namespace MapEditor
{
    public class BeatmapModeManager : MonoBehaviour
    {
        [SerializeField] private Timeline timeline;
        
        // TODO сделано по тупому с MainSelect.Selected
        private void Update()
        {
            if (BeatmapEditorContext.Mode == BeatmapEditorContext.ToolMode.Global &&
                MainSelect.Selected.Count == 0)
            {
                BeatmapEditorContext.Reset();
                return;
            }
            
            if (BeatmapEditorContext.Mode == BeatmapEditorContext.ToolMode.Preview &&
                MainSelect.Selected.Count > 0 &&
                MainSelect.Selected[0].GetComponent<MonoObject>() != null)
            {
                BeatmapEditorContext.Trigger = null;
                BeatmapEditorContext.Mode = BeatmapEditorContext.ToolMode.Global;
                timeline.Move(0);
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                ResetTrigger();

                // Режим триггера
                if (MainSelect.Selected.Count > 0 &&
                    MainSelect.Selected[0].gameObject.TryGetComponent(out IEffectTriggerPart part) &&
                    BeatmapEditorContext.Trigger != part.Parent)
                {
                    BeatmapEditorContext.Trigger = part.Parent;
                    BeatmapEditorContext.Mode = BeatmapEditorContext.ToolMode.Trigger;
                    BeatmapEditorContext.UpdateTrigger(timeline);
                }
                // Глобальный режим
                else
                {
                    BeatmapEditorContext.Trigger = null;
                    BeatmapEditorContext.Mode = BeatmapEditorContext.ToolMode.Global;
                    timeline.Move(0);
                }
            }
        }

        private void ResetTrigger()
        {
            if (BeatmapEditorContext.Trigger == null) return;
            
            foreach (var property in BeatmapEditorContext.Trigger.Timestamps.Keys)
            {
                property.Apply(property.GetDefault());   
            }
            if (BeatmapEditorContext.Trigger.TryGetComponent(out OutlinedObject outlined))
            {
                outlined.OutlineMode = OutlinedObject.Mode.OutlineHidden;
            }
        }
    }
}