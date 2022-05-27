using System;
using UnityEngine;
using VisualEffect.Property;

namespace VisualEffect.Object
{
    public class SkyObject : MonoObject
    {
        [VisualPropertyAttribute]
        private SkyColorProperty _skyColor;
        
        [VisualPropertyAttribute]
        private HorizonColorProperty _horizonColor;

        // Start is called before the first frame update
        private void Awake()
        {
            var skybox = gameObject.GetComponent<Skybox>();
            _skyColor = new SkyColorProperty(skybox);
            _horizonColor = new HorizonColorProperty(skybox);
        }

        // Update is called once per frame
        private void Update()
        {
            float red = (float) (Math.Sin(Time.time) * 2) / 2;
            float green = (float) (Math.Sin(Time.time * 4) + 1) / 2;
            float blue = (float) (Math.Sin(Time.time * 8) + 1) / 2;
            _horizonColor.Value = new Color(red, green, blue);
        }
        
        // Цвет неба
        private class SkyColorProperty : AbstractColorProperty
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
        private class HorizonColorProperty : AbstractColorProperty
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
