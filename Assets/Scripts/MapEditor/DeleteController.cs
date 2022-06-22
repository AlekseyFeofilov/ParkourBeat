using Beatmap.Object;
using Beatmap.Trigger;
using MapEditor.Select;
using MapEditor.Trigger;
using UnityEngine;
using VisualEffect.Object;

namespace Beatmap
{
    public class DeleteController : MonoBehaviour
    {
        [SerializeField] private ObjectManager objectManager;
        [SerializeField] private TriggerManager triggerManager;
        [SerializeField] private MainSelect mainSelect;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete)) DeleteSelected();
        }

        private void DeleteSelected()
        {
            GameObject selected = MainSelect.SelectedObj.gameObject;

            if (selected.TryGetComponent(out MonoObject monoObject))
            {
                mainSelect.Deselect();
                objectManager.RemoveObject(monoObject);
            }
            else if (selected.TryGetComponent(out IEffectTriggerPart part))
            {
                mainSelect.Deselect();
                triggerManager.RemoveEffectTrigger(part.Parent);
            }
        }
    }
}