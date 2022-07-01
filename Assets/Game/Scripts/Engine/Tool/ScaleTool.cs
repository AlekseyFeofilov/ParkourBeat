using System.Collections.Generic;
using Game.Scripts.Engine.Api.Listener;
using Game.Scripts.Engine.Manager;
using UnityEngine;

namespace Game.Scripts.Engine.Tool
{
    public class ScaleTool : Tool, ITool
    {
        private IScalable _scalable;

        protected override void AddChangeHandler(GameObject selected)
        {
            _scalable = selected.GetComponent<IScalable>();
        }

        protected override bool OnBegin(GameObject selected)
        {
            return selected.TryGetComponent(out IScalable scalable) && scalable.OnBeginScale();
        }

        protected override bool ChangeRequest(KeyValuePair<GameObject, Transform> data)
        {
            var change = ((ITool)this).GetChange(ScaledSpeed, tag);
            
            if (!data.Key.TryGetComponent(out IScalable scalable) || scalable.OnScale(change)) return true;

            SelectManager.Deselect(data.Key);
            Data.Remove(data.Key);

            return false;
        }
        
        protected override void Change()
        {
            ((ITool)this).Change(((ITool)this).GetChange(ScaledSpeed, tag));
        }

        protected override bool OnEnd(GameObject outlinedObject)
        {
            return !outlinedObject.TryGetComponent(out IScalable scalable) || scalable.OnEndScale();
        }
        
        void ITool.Change(Vector3 change) => ToolManager.Scale(change);
    }
}
