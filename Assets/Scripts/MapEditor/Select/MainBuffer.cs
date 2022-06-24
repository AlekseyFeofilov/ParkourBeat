using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MapEditor.Select
{
    public class MainBuffer : MonoBehaviour
    {
        private KeyValuePair<List<OutlinedObject>, GameObject> _buffer;
        private Camera _camera;

        [SerializeField] private UnityEvent<KeyValuePair<List<OutlinedObject>, GameObject>> @event;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
            {
                _buffer = new KeyValuePair<List<OutlinedObject>, GameObject>(MainSelect.Selected, _camera.gameObject);
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V))
            {
                @event.Invoke(_buffer);
            }
        }
    }
}
