using MapEditor.Select;
using UnityEngine;

namespace MapEditor.Tools
{
    public abstract class Tool : ActivatingObject
    {
        private bool _activated;

        [SerializeField] private float speedBooster = 1f;
        [SerializeField] protected float rotateSpeed = 1f;

        protected float Speed;
        protected float ScaledSpeed;

        private float _lenght;
        private Vector3 _projection;
        private Vector3 _offset;

        private Camera _camera;

        private Transform _previousTransform;

        protected override void Start()
        {
            base.Start();

            _camera = Camera.main;
        }

        protected override void OnMouseDown()
        {
            if (!OnBegin()) return;

            base.OnMouseDown();
            _previousTransform = MainSelect.SelectedObj.transform;
            _activated = true;
            CalculateProperties();
            _offset = Input.mousePosition;
        }

        protected override void OnMouseUp()
        {
            if (!OnEnd())
            {
                var selectedObjTransform = MainSelect.SelectedObj.transform;
                selectedObjTransform.position = _previousTransform.position;
                selectedObjTransform.rotation = _previousTransform.rotation;
                selectedObjTransform.localScale = _previousTransform.localScale;
            }

            base.OnMouseUp();
            _activated = false;
        }

        private void Update()
        {
            if (!_activated) return;

            if (Vector3.Distance(MainSelect.SelectedObj.transform.position, _previousTransform.position) > 100)
            {
                OnMouseUp();
                return;
            }

            CalculateProperties();
            CalculateChanges();

            OnChange();
        }
        
        private void CalculateProperties()
        {
            var end = transform.GetChild(1).transform.position;
            var begin = transform.GetChild(0).transform.position;
            _lenght = (end - begin).magnitude;

            var endProjection = _camera.WorldToScreenPoint(end);
            var beginProjection = _camera.WorldToScreenPoint(begin);
            _projection = endProjection - beginProjection;
            _projection.z = 0;
        }

        private void CalculateChanges()
        {
            var movement = Input.mousePosition - _offset;
            var projection = Vector3.Project(movement, _projection);

            var sign = Vector3.Angle(projection, _projection) < 90 ? 1 : -1;

            Speed = projection.magnitude * sign;
            ScaledSpeed = Speed * _lenght * speedBooster / _projection.magnitude;

            _offset = Input.mousePosition;
        }

        protected abstract bool OnBegin();

        protected abstract void OnChange();

        protected abstract bool OnEnd();
    }
}