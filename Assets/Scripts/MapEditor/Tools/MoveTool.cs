using MapEditor.ChangeableInterfaces;
using MapEditor.Select;
using UnityEngine;

namespace MapEditor.Tools
{
    public class MoveTool : Tool, ITool
    {
        private IMovable _movable;

        protected override void Start()
        {
            base.Start();
            
            if (!MainSelect.SelectedObj.TryGetComponent(out _movable))
            {
                MainTools.Hide();
            }
        }

        protected override bool OnBegin()
        {
            return _movable.OnBeginMove();
        }

        protected override void OnChange()
        {
            var change = ((ITool)this).GetChange(ScaledSpeed, tag);
            if (!_movable.OnMove(change)) return;
            ((ITool)this).Change(change);
        }
        
        

        protected override bool OnEnd()
        {
            return _movable.OnEndMove();
        }

        void ITool.Change(Vector3 change) => MainTools.Move(change);
    }
}