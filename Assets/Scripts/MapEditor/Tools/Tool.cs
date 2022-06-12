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

        protected override void Start()
        {
            base.Start();

            _camera = Camera.main;
        }

        protected override void OnMouseDown()
        {
            base.OnMouseDown();

            MainTools.Save();
            _activated = true;
            CalculateProperties();
            _offset = Input.mousePosition;
        }

        protected override void OnMouseUp()
        {
            base.OnMouseUp();
            
            _activated = false;
        }

        private void Update()
        {
            if (!_activated) return;
            
            CalculateProperties();
            CalculateChanges();
            Change();
        }

        protected abstract void Change();
        
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
            ScaledSpeed =  Speed  * _lenght * speedBooster / _projection.magnitude;
            
            _offset = Input.mousePosition;
        }
    }
}