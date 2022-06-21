using MapEditor.ChangeableInterfaces;
using MapEditor.Select;
using UnityEngine;

namespace MapEditor.Tools
{
    public class RotateTool : Tool, ITool
    {
        private int _clicked;
        private float _clickTime;
        private const float ClickDelay = 0.5f;

        private IRotatable _rotatable;

        protected override void Start()
        {
            base.Start();
            _rotatable = MainSelect.SelectedObj.GetComponent<IRotatable>();
        }

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

        protected override bool OnBegin()
        {
            return _rotatable.OnBeginRotate();
        }

        protected override void OnChange()
        {
            var change = ((ITool)this).GetChange(Speed * rotateSpeed, tag);
            if (!_rotatable.OnRotate(change)) return;
            ((ITool)this).Change(change);
        }

        protected override bool OnEnd()
        {
            return _rotatable.OnEndRotate();
        }

        void ITool.Change(Vector3 change) => MainTools.Rotate(change);
    }
}