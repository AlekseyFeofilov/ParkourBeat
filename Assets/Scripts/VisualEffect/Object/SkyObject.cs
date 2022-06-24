using MapEditor;
using MapEditor.ChangeableInterfaces;
using MapEditor.Event;
using MapEditor.Select;
using MapEditor.Tools;
using UnityEngine;
using VisualEffect.Property;

namespace VisualEffect.Object
{
    public class SkyObject : MonoObject, ISelectable, IColorable
    {
        [SerializeField] private Skybox skybox;
        [SerializeField] private MainTools mainTools;
        
        [VisualPropertyAttribute(Id = "Color")]
        public SkyColorProperty Color;

        private void Awake()
        {
            Color = new SkyColorProperty(skybox);
        }

        public void OnSelect(SelectEvent @event)
        {
            mainTools.ToolMode = MainTools.Mode.ColorTool;
        }

        public bool OnChange(Color color)
        {
            BeatmapEditorContext.SetPropertyValue(Color, color);
            Color.Apply(color);
            return false;
        }

        public void OnBeginColor(ColorBeginEvent @event)
        {
            @event.StartColor = Color.Get();
        }

        // Цвет неба
        public class SkyColorProperty : AbstractColorProperty
        {
            private static readonly int SkyColor = Shader.PropertyToID("_SkyColor");
            private readonly Skybox _skybox;

            public SkyColorProperty(Skybox skybox)
            {
                _skybox = skybox;
            }

            protected override void Apply(Color state)
            {
                _skybox.material.SetColor(SkyColor, state);
            }

            public Color Get()
            {
                return _skybox.material.GetColor(SkyColor);
            }
        }
    }
}
