using UnityEngine;

namespace MapEditor.Tools
{
    public class RotateTool : Tool, ITool
    {
        protected override void Change() => ((ITool)this).Change(Speed * rotateSpeed, tag);

        void ITool.ChangeOx(float speed)
        {
            MainTools.Rotate(speed * Vector3.right);
        }

        void ITool.ChangeOy(float speed)
        {
            MainTools.Rotate(speed * Vector3.up);
        }

        void ITool.ChangeOz(float speed)
        {
            MainTools.Rotate(speed * Vector3.forward);
        }
    }
}
