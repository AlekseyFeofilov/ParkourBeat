using System.Collections.Generic;
using MapEditor.ChangeableInterfaces;
using UnityEngine;

namespace MapEditor.Tools
{
    public class ScaleTool : Tool, ITool
    {
        private IScalable _scalable;

        protected override void AddChangeHandler(OutlinedObject selected)
        {
            _scalable = selected.GetComponent<IScalable>();
        }

        protected override bool OnBegin(OutlinedObject selected)
        {
            return selected.TryGetComponent(out IScalable scalable) && scalable.OnBeginScale();
        }

        protected override void ChangeRequest(KeyValuePair<OutlinedObject, Transform> data)
        {
            var change = ((ITool)this).GetChange(ScaledSpeed, tag);
            if (_scalable.OnScale(change)) return;
            
            MainSelect.Deselect(data.Key);
            Data.Remove(data.Key);
        }
        
        protected override void Change()
        {
            ((ITool)this).Change(((ITool)this).GetChange(ScaledSpeed, tag));
        }

        protected override bool OnEnd(OutlinedObject outlinedObject)
        {
            return _scalable.OnEndScale();
        }
        
        void ITool.Change(Vector3 change) => MainTools.Scale(change);
    }
}
