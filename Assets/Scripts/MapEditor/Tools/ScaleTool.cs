using MapEditor.ChangeableInterfaces;
using MapEditor.Select;
using UnityEngine;

namespace MapEditor.Tools
{
    public class ScaleTool : Tool, ITool
    {
        private IScalable _scalable;

        protected override void Start()
        {
            base.Start();
            
            if (!MainSelect.SelectedObj.TryGetComponent(out _scalable))
            {
                MainTools.Hide();
            }
        }

        protected override bool OnBegin()
        {
            return _scalable.OnBeginScale();
        }

        protected override void OnChange()
        {
            var change = ((ITool)this).GetChange(ScaledSpeed, tag);
            if (!_scalable.OnScale(change)) return;
            ((ITool)this).Change(change);

        }

        protected override bool OnEnd()
        {
            return _scalable.OnEndScale();
        }
        
        void ITool.Change(Vector3 change) => MainTools.Scale(change);
    }
}
