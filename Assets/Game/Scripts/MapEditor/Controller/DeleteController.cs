using System.Collections.Generic;
using Game.Scripts.Engine.Manager;
using Game.Scripts.Map.Manager;
using Game.Scripts.Map.VisualEffect.Object;
using Game.Scripts.MapEditor.Trigger;
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
            foreach (var selected in new List<GameObject>(SelectManager.Selected))
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