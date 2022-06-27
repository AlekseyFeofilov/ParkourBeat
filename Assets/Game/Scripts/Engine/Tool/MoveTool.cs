using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Engine.Api.Listener;
using Game.Scripts.Engine.Manager;
using UnityEngine;

namespace Game.Scripts.Engine.Tool
{
    public class MoveTool : Tool, ITool
    {
        private IMovable _movable;
        private Vector3 _startPosition;

        protected override void OnMouseDown()
        {
            base.OnMouseDown();

            _startPosition = SelectManager.Selected.First().transform.parent.parent.position;
        }

        protected override void AddChangeHandler(GameObject selected)
        {
            _movable = selected.GetComponent<IMovable>();
        }

        protected override bool OnBegin(GameObject selected)
        {
            return selected.TryGetComponent(out IMovable movable) && movable.OnBeginMove();
        }

        protected override bool ChangeRequest(KeyValuePair<GameObject, Transform> data)
        {
            var change = ((ITool)this).GetChange(ScaledSpeed, tag);

            if (!data.Key.TryGetComponent(out IMovable movable) || movable.OnMove(change)) return true;

            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            SelectManager.Deselect(data.Key);
            Data.Remove(data.Key);

            return false;
        }

        protected override void Change() => ((ITool)this).Change(((ITool)this).GetChange(ScaledSpeed, tag));

        protected override bool OnEnd(GameObject selected)
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            return !selected.TryGetComponent(out IMovable movable) || movable.OnEndMove();
        }

        void ITool.Change(Vector3 change) => ToolManager.Move(_startPosition, change);
    }
}