using System.Collections;
using Game.Scripts.Engine.Api.Event;
using Game.Scripts.Engine.Api.Listener;
using Game.Scripts.Map.VisualEffect.Property;
using Game.Scripts.MapEditor;
using Libraries.QuickOutline.Scripts;
using UnityEngine;

namespace Game.Scripts.Map.VisualEffect.Object
{
    public class CubeObject : MonoObject, ISelectable, IMovable, IRotatable, IScalable, IColorable
    {
        [VisualProperty(Id = "Position")]
        public PositionProperty Position;
        
        [VisualProperty(Id = "Rotation")]
        public RotationProperty Rotation;
        
        [VisualProperty(Id = "Scale")]
        public ScaleProperty Scale;
        
        [VisualProperty(Id = "Color")]
        public ColorProperty Color;

        private void Awake()
        {
            Renderer renderer = GetComponent<Renderer>();
            Transform transform1 = transform;
            
            Position = new PositionProperty(transform1);
            Rotation = new RotationProperty(transform1);
            Scale = new ScaleProperty(transform1);
            Color = new ColorProperty(renderer.material);
        }

        public void OnDeselect(DeselectEvent @event)
        {
            if (BeatmapEditorContext.Mode == BeatmapEditorContext.ToolMode.Trigger &&
                BeatmapEditorContext.Trigger.Objects.Contains(this))
            {
                StartCoroutine(SetRedOutlineDelayed());
            }
        }

        private IEnumerator SetRedOutlineDelayed()
        {
            yield return 0;
            if (!TryGetComponent(out OutlinedObject outlinedObject))
            {
                outlinedObject = gameObject.AddComponent<OutlinedObject>();
            }
            outlinedObject.OutlineColor = UnityEngine.Color.red;
            outlinedObject.OutlineMode = OutlinedObject.Mode.OutlineAll;
            outlinedObject.OutlineWidth = 10;
        }

        public bool OnEndMove()
        {
            BeatmapEditorContext.SetPropertyValue(Position, transform.position);
            return true;
        }

        public bool OnEndRotate()
        {
            BeatmapEditorContext.SetPropertyValue(Rotation, transform.rotation.eulerAngles);
            return true;
        }

        public bool OnEndScale()
        {
            BeatmapEditorContext.SetPropertyValue(Scale, transform.localScale);
            return true;
        }

        public bool OnChange(Color color)
        {
            BeatmapEditorContext.SetPropertyValue(Color, color);
            return true;
        }

        // Позиция
        public class PositionProperty : AbstractVector3Property
        {
            private readonly Transform _transform;

            public PositionProperty(Transform transform)
            {
                _transform = transform;
                Default = _transform.position;
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
                Default = _transform.rotation.eulerAngles;
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
                Default = _transform.localScale;
            }

            protected override void Apply(Vector3 state)
            {
                _transform.localScale = state;
            }
        }
        
        // Цвет
        public class ColorProperty : AbstractColorProperty
        {
            private readonly Material _material;

            public ColorProperty(Material material)
            {
                _material = material;
                Default = _material.color;
            }

            protected override void Apply(Color state)
            {
                _material.color = state;
            }
        }
    }
}