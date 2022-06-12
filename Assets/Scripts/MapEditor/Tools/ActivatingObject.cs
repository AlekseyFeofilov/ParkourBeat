using System.Collections.Generic;
using UnityEngine;

namespace MapEditor.Tools
{
    public class ActivatingObject : MonoBehaviour
    {
        [SerializeField] private Material @default;
        [SerializeField] private Material active;
        [SerializeField] private Material inactive;

        public List<Renderer> renderers;
        public List<ActivatingObject> otherObject;
        
        protected virtual void Start()
        {
            foreach (Transform child in transform)
            {
                renderers.Add(child.GetComponent<Renderer>());
            }

            foreach (Transform child in transform.parent)
            {
                ActivatingObject obj;
                if ((obj = child.GetComponent<ActivatingObject>()) != this)
                {
                    otherObject.Add(obj);
                }
            }
        }

        protected virtual void OnMouseDown()
        {
            Activate();

            foreach (var obj in otherObject)
            {
                obj.Deactivate();
            }
        }

        protected virtual void OnMouseUp()
        {
            Default();
            
            foreach (var obj in otherObject)
            {
                obj.Default();
            }
        }

        private void Activate()
        {
            foreach (var render in renderers)
            {
                render.material = active;
            }
        }

        private void Deactivate()
        {
            foreach (var render in renderers)
            {
                render.material = inactive;
            }
        }

        private void Default()
        {
            foreach (var render in renderers)
            {
                render.material = @default;
            }
        }
    }
}