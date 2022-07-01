using System.Collections.Generic;
using Game.Scripts.Engine.Api.Listener;
using Game.Scripts.Engine.Manager;
using UnityEngine;

namespace Game.Scripts.Engine.Tool
{
    public class RotateTool : Tool, ITool
    {
        private IRotatable _rotatable;

        protected override void AddChangeHandler(GameObject selected)
        {
            _rotatable = selected.GetComponent<IRotatable>();
        }

        protected override bool OnBegin(GameObject selected)
        {
            return selected.TryGetComponent(out IRotatable rotatable) && rotatable.OnBeginRotate();
        }

        protected override bool ChangeRequest(KeyValuePair<GameObject, Transform> data)
        {
            var change = ((ITool)this).GetChange(Speed * rotateSpeed, tag);
            
            if (!data.Key.TryGetComponent(out IRotatable rotatable) || rotatable.OnRotate(change)) return true;

            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            Deselect(data.Key);
            return false;
        }

        private void Deselect(GameObject selected)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            SelectManager.Deselect(selected);
            Data.Remove(selected);
        }

        protected override void Change() => ((ITool)this).Change(((ITool)this).GetChange(Speed * rotateSpeed, tag));

        protected override bool OnEnd(GameObject outlinedObject)
        {
            return !outlinedObject.TryGetComponent(out IRotatable rotatable) || rotatable.OnEndRotate();
        }

        void ITool.Change(Vector3 change) => ToolManager.Rotate(change);
    }
}