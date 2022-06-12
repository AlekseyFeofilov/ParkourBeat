using UnityEngine;

namespace MapEditor.Tools
{
    public class ScaleTool : Tool, ITool
    {
        protected override void Change() => ((ITool)this).Change(ScaledSpeed, tag);

        void ITool.ChangeOx(float speed)
        {
            MainTools.Scale(speed * Vector3.right);
        }

        void ITool.ChangeOy(float speed)
        {
            MainTools.Scale(speed * Vector3.up);
        }

        void ITool.ChangeOz(float speed)
        {
            MainTools.Scale(speed * Vector3.forward);
        }
    }
}
