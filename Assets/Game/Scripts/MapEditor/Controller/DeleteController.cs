using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Engine.Manager;
using Game.Scripts.Map.Manager;
using Game.Scripts.Map.VisualEffect.Object;
using Game.Scripts.MapEditor.Trigger;
using Libraries.QuickOutline.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.MapEditor.Controller
{
    public class DeleteController : MonoBehaviour
    {
        [SerializeField] private ObjectManager objectManager;
        [SerializeField] private TriggerManager triggerManager;
        [FormerlySerializedAs("mainSelect")] [SerializeField] private SelectManager selectManager;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Delete)) DeleteSelected();
        }

        private void DeleteSelected()
        {
            foreach (GameObject selected in new List<OutlinedObject>(SelectManager.Selected).Select(e => e.gameObject))
            {
                if (selected.TryGetComponent(out MonoObject monoObject))
                {
                    selectManager.Deselect();
                    objectManager.RemoveObject(monoObject);
                }
                else if (selected.TryGetComponent(out IEffectTriggerPart part))
                {
                    selectManager.Deselect();
                    triggerManager.RemoveEffectTrigger(part.Parent);
                }
            }
        }
    }
}