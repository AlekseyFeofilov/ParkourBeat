using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MapEditor.Select
{
    public class MainBuffer : MonoBehaviour
    {
        private Buffer _buffer;
        private Camera _camera;

        [SerializeField] private UnityEvent<Buffer> @event;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            {
                Dictionary<GameObject, Vector3> objects = new();
                foreach (OutlinedObject outlinedObject in MainSelect.Selected)
                {
                    objects[outlinedObject.gameObject] = outlinedObject.transform.position * 1;
                }
                _buffer = new Buffer(objects, _camera.transform.position * 1);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
            {
                @event.Invoke(_buffer);
            }
        }
    }

    public class Buffer
    {
        public readonly Dictionary<GameObject, Vector3> Objects;
        public readonly Vector3 Center;

        public Buffer(Dictionary<GameObject, Vector3> objects, Vector3 center)
        {
            Objects = objects;
            Center = center;
        }
    }
}
