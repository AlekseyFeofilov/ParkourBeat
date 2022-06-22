using Beatmap.Object;
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

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete)) DeleteSelected();
        }

        private void DeleteSelected()
        {
            GameObject selected = MainSelect.SelectedObj.gameObject;

            if (selected.TryGetComponent(out MonoObject monoObject))
            {
                objectManager.RemoveObject(monoObject);
            }
            else if (selected.TryGetComponent(out EffectTrigger effectTrigger))
            {
                triggerManager.RemoveEffectTrigger(effectTrigger);
            }
            else return;

            // TODO MainSelect.SelectedObj = null;
        }
    }
}