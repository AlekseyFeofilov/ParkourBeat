using MapEditor.Select;
using MapEditor.Trigger;
using UnityEngine;

namespace MapEditor
{
    public class BeatmapModeManager : MonoBehaviour
    {
        [SerializeField] private Timeline timeline;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ResetTrigger();

                // Режим триггера
                if (MainSelect.SelectedObj != null &&
                    MainSelect.SelectedObj.gameObject.TryGetComponent(out IEffectTriggerPart part) &&
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

            // Превью режим
            if (Input.GetKeyDown(KeyCode.G))
            {
                ResetTrigger();
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
            if (BeatmapEditorContext.Trigger.TryGetComponent(out OutlinedObject outlined))
            {
                outlined.OutlineMode = OutlinedObject.Mode.OutlineHidden;
            }
        }
    }
}