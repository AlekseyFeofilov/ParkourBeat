using UnityEngine;

namespace MapEditor.Tools
{
    public class MoveTool : Tool, ITool
    {
        protected override void Change() => ((ITool)this).Change(ScaledSpeed, tag);

        void ITool.ChangeOx(float speed)
        {
            MainTools.Move(speed * Vector3.right);
        }

        void ITool.ChangeOy(float speed)
        {
            MainTools.Move(speed * Vector3.up);
        }

        void ITool.ChangeOz(float speed)
        {
            MainTools.Move(speed * Vector3.forward);
        }
    }
}