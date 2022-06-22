using System;
using UnityEngine;

namespace Behaviour
{
    public class AdaptiveScale : MonoBehaviour
    {
        [SerializeField] private float minScale;
        [SerializeField] private float maxScale = float.MaxValue;
        [SerializeField] private float size = .025f;
        
        [SerializeField] private bool ignoreDistanceX;
        [SerializeField] private bool ignoreDistanceY;
        [SerializeField] private bool ignoreDistanceZ;
        
        [SerializeField] private bool ignoreScaleX;
        [SerializeField] private bool ignoreScaleY;
        [SerializeField] private bool ignoreScaleZ;
        
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            Vector3 cameraPosition = _camera.transform.position;
            Vector3 position = transform.position;
            
            float scale = (float) Math.Sqrt(
                (ignoreDistanceX ? 0 : Math.Pow(cameraPosition.x - position.x, 2)) +
                (ignoreDistanceY ? 0 : Math.Pow(cameraPosition.y - position.y, 2)) +
                (ignoreDistanceZ ? 0 : Math.Pow(cameraPosition.z - position.z, 2))
            ) / (1 / size);
            scale = Mathf.Max(minScale, Mathf.Min(maxScale, scale));

            var transform1 = transform;
            var localScale = transform1.localScale;

            float scaleX = ignoreScaleX ? localScale.x : scale;
            float scaleY = ignoreScaleY ? localScale.y : scale;
            float scaleZ = ignoreScaleZ ? localScale.z : scale;
            
            localScale = new Vector3(scaleX, scaleY, scaleZ);
            transform1.localScale = localScale;
        }
    }
}