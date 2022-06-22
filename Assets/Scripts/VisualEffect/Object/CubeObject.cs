﻿using MapEditor.ChangeableInterfaces;
using UnityEngine;
using VisualEffect.Property;

namespace VisualEffect.Object
{
    public class CubeObject : MonoObject, IMovable, IRotatable, IScalable
    {
        [VisualPropertyAttribute(Id = "Position")]
        public PositionProperty Position;
        
        [VisualPropertyAttribute(Id = "Rotation")]
        public RotationProperty Rotation;
        
        [VisualPropertyAttribute(Id = "Scale")]
        public ScaleProperty Scale;

        private void Awake()
        {
            var transform1 = transform;
            Position = new PositionProperty(transform1);
            Rotation = new RotationProperty(transform1);
            Scale = new ScaleProperty(transform1);
        }
        
        public bool OnEndMove()
        {
            Position.Default = transform.position;
            return true;
        }

        public bool OnEndRotate()
        {
            Rotation.Default = transform.rotation.eulerAngles;
            return true;
        }

        public bool OnEndScale()
        {
            Scale.Default = transform.localScale;
            return true;
        }

        // Позиция
        public class PositionProperty : AbstractVector3Property
        {
            private readonly Transform _transform;

            public PositionProperty(Transform transform)
            {
                _transform = transform;
            }

            protected override void Apply(Vector3 state)
            {
                _transform.position = state;
            }
        }
        
        // Вращение
        public class RotationProperty : AbstractVector3Property
        {
            private readonly Transform _transform;

            public RotationProperty(Transform transform)
            {
                _transform = transform;
            }

            protected override void Apply(Vector3 state)
            {
                _transform.rotation = Quaternion.Euler(state);
            }
        }
        
        // Масштаб
        public class ScaleProperty : AbstractVector3Property
        {
            private readonly Transform _transform;

            public ScaleProperty(Transform transform)
            {
                _transform = transform;
            }

            protected override void Apply(Vector3 state)
            {
                _transform.localScale = state;
            }
        }
    }
}