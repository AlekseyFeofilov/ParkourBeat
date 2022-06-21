using UnityEngine;

namespace MapEditor.Tools
{
    public sealed class RotateTool : Tool, ITool
    {
        private int _clicked;
        private float _clickTime;
        private const float ClickDelay = 0.5f;

        protected override void OnMouseDown()
        {
            base.OnMouseDown();

            if (Time.time - _clickTime > ClickDelay)
            {
                _clicked = 0;
            }

            _clicked++;
            _clickTime = Time.time;

            if (_clicked == 2)
            {
                OnDoubleClick();
            }
        }

        private void OnDoubleClick()
        {
            var rotation = transform.parent.parent.rotation.eulerAngles;

            switch (tag)
            {
                case "OX":
                    MainTools.SetRotation(Quaternion.Euler(0, rotation.y, rotation.z));
                    break;

                case "OY":
                    MainTools.SetRotation(Quaternion.Euler(rotation.x, 0, rotation.z));
                    break;

                case "OZ":
                    MainTools.SetRotation(Quaternion.Euler(rotation.x, rotation.y,0));
                    break;
            }
        }

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