using UnityEngine;
using VisualEffect.Property;

namespace VisualEffect.Object
{
    public class SkyObject : MonoObject
    {
        [VisualPropertyAttribute]
        public SkyColorProperty SkyColor;
        
        [VisualPropertyAttribute]
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

            public override Color Value
            {
                get => _skybox.material.GetColor(SkyColor);
                set => _skybox.material.SetColor(SkyColor, value);
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

            public override Color Value
            {
                get => _skybox.material.GetColor(HorizonColor);
                set => _skybox.material.SetColor(HorizonColor, value);
            }
        }
    }
}
