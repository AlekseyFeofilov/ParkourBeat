using UnityEngine;

namespace Game.Scripts.Engine.Visual
{
    public class VisuallyNotScalableObject : MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }
        
        private void Update()
        {
            var scale = DistanceBetweenCameraAndTool();
            transform.localScale = new Vector3(scale, scale, scale) / 3;
        }
        
        private float DistanceBetweenCameraAndTool()
        {
            return Vector3.Magnitude(transform.position - _camera.transform.position);
        }
    }
}