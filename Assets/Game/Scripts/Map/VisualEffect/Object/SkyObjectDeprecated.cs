using Game.Scripts.Map.VisualEffect.Property;
using UnityEngine;

namespace Game.Scripts.Map.VisualEffect.Object
{
    public class SkyObjectDeprecated : MonoObject
    {
        [VisualProperty(Id = "SkyColor")]
        public SkyColorProperty SkyColor;
        
        [VisualProperty(Id = "HorizonColor")]
        public HorizonColorProperty HorizonColor;
        
        private void Awake()
        {
            var skybox = gameObject.GetComponent<Skybox>();
            SkyColor = new SkyColorProperty(skybox);
            HorizonColor = new HorizonColorProperty(skybox);
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
        }
        
        // Цвет горизонта
        public class HorizonColorProperty : AbstractColorProperty
        {
            private static readonly int HorizonColor = Shader.PropertyToID("_HorizonColor");
            private readonly Skybox _skybox;

            public HorizonColorProperty(Skybox skybox)
            {
                _skybox = skybox;
            }

            protected override void Apply(Color state)
            {
                _skybox.material.SetColor(HorizonColor, state);
                RenderSettings.fogColor = state;
            }
        }
    }
}
