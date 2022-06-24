using System.Collections.Generic;
using MapEditor.ChangeableInterfaces;
using UnityEngine;

namespace MapEditor.Tools
{
    public class RotateTool : Tool, ITool
    {
        private int _clicked;
        private float _clickTime;
        private const float ClickDelay = 0.5f;

        private IRotatable _rotatable;

        protected override void AddChangeHandler(OutlinedObject selected)
        {
            _rotatable = selected.GetComponent<IRotatable>();
        }

        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RotationReset(true, true, true);
            }

            base.Update();
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
            switch (tag)
            {
                case "OX":
                    RotationReset(true, false, false);
                    break;

                case "OY":
                    RotationReset(false, true, false);
                    break;

                case "OZ":
                    RotationReset(false, false, true);
                    break;
            }
        }

        private static void RotationReset(bool resetOx, bool resetOy, bool resetOz)
        {
            MainTools.RotationReset(resetOx, resetOy, resetOz);
            MainTools.UpdateTool();
        }

        protected override bool OnBegin(OutlinedObject selected)
        {
            return selected.TryGetComponent(out IRotatable rotatable) && rotatable.OnBeginRotate();
        }

        protected override void ChangeRequest(KeyValuePair<OutlinedObject, Transform> data)
        {
            var change = ((ITool)this).GetChange(Speed * rotateSpeed, tag);
            
            if (!data.Key.TryGetComponent(out IRotatable rotatable) || rotatable.OnRotate(change)) return;

            MainSelect.Deselect(data.Key);
            Data.Remove(data.Key);
        }

        protected override void Change() => ((ITool)this).Change(((ITool)this).GetChange(Speed * rotateSpeed, tag));

        protected override bool OnEnd(OutlinedObject outlinedObject)
        {
            return !outlinedObject.TryGetComponent(out IRotatable rotatable) || rotatable.OnEndRotate();
        }

        void ITool.Change(Vector3 change) => MainTools.Rotate(change);
    }
}