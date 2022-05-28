using System;
using UnityEngine;
using VisualEffect.Function;
using VisualEffect.Property;

namespace VisualEffect.Object
{
    public class PlacedObject : MonoObject
    {
        [VisualPropertyAttribute]
        private PositionProperty _position;
        
        [VisualPropertyAttribute]
        private RotationProperty _rotation;
        
        [VisualPropertyAttribute]
        private ScaleProperty _scale;

        private void Awake()
        {
            var transform1 = transform;
            _position = new PositionProperty(transform1);
            _rotation = new RotationProperty(transform1);
            _scale = new ScaleProperty(transform1);
        }

        private void Update()
        {
            // ТЕСТ
            switch (Time.frameCount)
            {
                case 1000:
                    _position.BeginTransition(_position.Value + new Vector3(0, 1, 0), 0.5f, ITimingFunction.Ease);
                    break;
                case 2000:
                    _scale.BeginTransition(_scale.Value * 1.25f, 0f, ITimingFunction.Linear);
                    _scale.BeginTransition(new Vector3(1, 1, 1), 0.3f, ITimingFunction.Linear);
                    break;
                case 2500:
                    _rotation.BeginTransition(_position.Value + new Vector3(300, 30, 0), 8f, ITimingFunction.EaseOut);
                    break;
            }
        }

        // Позиция
        private class PositionProperty : AbstractVector3Property
        {
            private readonly Transform _transform;

            public PositionProperty(Transform transform)
            {
                _transform = transform;
            }

            public override Vector3 Value
            {
                get => _transform.position;
                set => _transform.position = value;
            }
        }
        
        // Вращение
        private class RotationProperty : AbstractVector3Property
        {
            private readonly Transform _transform;

            public RotationProperty(Transform transform)
            {
                _transform = transform;
            }

            public override Vector3 Value
            {
                get => _transform.rotation.eulerAngles;
                set => _transform.rotation = Quaternion.Euler(value);
            }
        }
        
        // Масштаб
        private class ScaleProperty : AbstractVector3Property
        {
            private readonly Transform _transform;

            public ScaleProperty(Transform transform)
            {
                _transform = transform;
            }

            public override Vector3 Value
            {
                get => _transform.localScale;
                set => _transform.localScale = value;
            }
        }
    }
}