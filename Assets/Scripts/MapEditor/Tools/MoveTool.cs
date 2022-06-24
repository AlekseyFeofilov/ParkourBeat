using System.Collections.Generic;
using MapEditor.ChangeableInterfaces;
using UnityEngine;

namespace MapEditor.Tools
{
    public class MoveTool : Tool, ITool
    {
        private IMovable _movable;
        
        protected override void AddChangeHandler(OutlinedObject selected)
        {
            _movable = selected.GetComponent<IMovable>();
        }

        protected override bool OnBegin(OutlinedObject selected)
        {
            return selected.TryGetComponent(out IMovable movable) && movable.OnBeginMove();
        }

        protected override void ChangeRequest(KeyValuePair<OutlinedObject, Transform> data)
        {
            var change = ((ITool)this).GetChange(ScaledSpeed, tag);

            if (!data.Key.TryGetComponent(out IMovable movable) || movable.OnMove(change)) return;

            MainSelect.Deselect(data.Key);
            Data.Remove(data.Key);
        }

        protected override void Change() => ((ITool)this).Change(((ITool)this).GetChange(ScaledSpeed, tag));

        protected override bool OnEnd(OutlinedObject selected)
        {
            return !selected.TryGetComponent(out IMovable movable) || movable.OnEndMove();
        }

        void ITool.Change(Vector3 change) => MainTools.Move(change);
    }
}