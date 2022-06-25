using System.Collections.Generic;
using Game.Scripts.Engine.Api.Listener;
using Game.Scripts.Engine.Manager;
using Libraries.QuickOutline.Scripts;
using UnityEngine;

namespace Game.Scripts.Engine.Tool
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

            SelectManager.Deselect(data.Key);
            Data.Remove(data.Key);
        }

        protected override void Change() => ((ITool)this).Change(((ITool)this).GetChange(ScaledSpeed, tag));

        protected override bool OnEnd(OutlinedObject selected)
        {
            return !selected.TryGetComponent(out IMovable movable) || movable.OnEndMove();
        }

        void ITool.Change(Vector3 change) => ToolManager.Move(change);
    }
}