using System.Collections.Generic;
using Game.Scripts.Engine.Select;
using Libraries.QuickOutline.Scripts;
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
                foreach (OutlinedObject outlinedObject in SelectManager.Selected)
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
}
