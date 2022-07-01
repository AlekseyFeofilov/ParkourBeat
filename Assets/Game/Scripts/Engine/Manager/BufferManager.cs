using System.Collections.Generic;
using Game.Scripts.Engine.Select;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Engine.Manager
{
    public class BufferManager : MonoBehaviour
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
                foreach (var obj in SelectManager.Selected)
                {
                    objects[obj.gameObject] = obj.transform.position * 1;
                }
                _buffer = new Buffer(objects, _camera.transform.position * 1);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
            {
                @event.Invoke(_buffer);
            }
        }
    }
}
