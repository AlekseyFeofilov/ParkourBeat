using System.Linq;
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
            foreach (GameObject selected in MainSelect.Selected.Select(e => e.gameObject))
            {
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
}