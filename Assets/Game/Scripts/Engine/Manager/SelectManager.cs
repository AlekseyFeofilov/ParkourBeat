using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Engine.Api.Event;
using Game.Scripts.Engine.Api.Listener;
using Game.Scripts.Engine.Select;
using Libraries.QuickOutline.Scripts;
using Unity.VisualScripting;
using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace Game.Scripts.Engine.Manager
{
    public class SelectManager : MonoBehaviour
    {
        public LayerMask selectableMask;
        public LayerMask toolMask;
        public GameObject emptyObject;

        private Camera _camera;

        [SerializeField] private ToolManager toolManager;

        private Transform _wrapper;

        private void Awake()
        {
            Selected = new List<GameObject>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        public static List<GameObject> Selected { get; private set; }

        // ReSharper disable twice Unity.PerformanceCriticalCodeNullComparison
        private void Select()
        {
            if (UIRaycaster.PointerIsOverUI(Input.mousePosition)) return;

            var obj = GetObjectByMousePosition();
            
            if (Selected.Contains(obj))
            {
                if (Input.GetKey(KeyCode.LeftControl)) Deselect(obj);
                return;
            }
            if (!Input.GetKey(KeyCode.LeftControl) && Selected != null) Deselect();

            Select(obj);
        }

        public void Select(List<GameObject> objects)
        {
            foreach (var obj in objects)
            {
                Select(obj);
            }
        }

        public void Select(GameObject obj)
        {
            if (obj == null) return;
            
            var @event = SelectRequest(obj);
            if (@event.Cancelled) return;

            Selected.Add(obj);
            Select(obj.transform.AddComponent<OutlinedObject>(), @event);
        }
        
        private void Select(OutlinedObject obj, SelectEvent @event)
        {
            if (Selected.Count == 1)
            {
                _wrapper = AddWrapper();
            }

            obj.transform.parent = _wrapper;
            obj.OutlineWidth = 10;
            obj.OutlineColor = @event.SelectColor;
        }

        private Transform AddWrapper()
        {
            var selectedTransform = Selected[0].transform;
            
            var mainWrapper = Instantiate(
                emptyObject,
                selectedTransform.position,
                selectedTransform.rotation
            ).transform;

            var wrapper = Instantiate(
                emptyObject,
                mainWrapper.position,
                mainWrapper.rotation
            ).transform;

            wrapper.parent = mainWrapper;
            
            mainWrapper.name = "Selection";
            wrapper.name = "Selected";
            toolManager.Activate();
            return wrapper;
        }

        private void RemoveWrapper()
        {
            Transform parent;
            var selectedTransform = Selected[0].transform;

            selectedTransform.parent = (parent = selectedTransform.parent).parent.parent;
            Destroy(parent.parent.gameObject);
            Destroy(parent.gameObject);
            toolManager.Deactivate();
        }

        private static SelectEvent SelectRequest(GameObject obj)
        {
            SelectEvent @event = new();

            if (obj.TryGetComponent(out ISelectable selectable))
            {
                selectable.OnSelect(@event);
            }

            return @event;
        }

        private static DeselectEvent DeselectRequest(GameObject obj)
        {
            DeselectEvent @event = new();

            if (obj.TryGetComponent(out ISelectable selectable))
            {
                selectable.OnDeselect(@event);
            }

            return @event;
        }

        public void Deselect(GameObject obj)
        {
            var @event = DeselectRequest(obj);
            if (@event.Cancelled) return;
            if(obj == Selected[0]) toolManager.Hide();
            
            if (Selected.Count == 1)
            {
                RemoveWrapper();
            }
            else
            {
                var objTransform = obj.transform;
                objTransform.parent = objTransform.parent.parent.parent;
            }

            Selected.Remove(obj);
            Destroy(obj.GetComponent<OutlinedObject>());
        }
        
        public void Deselect()
        {
            while (Selected.Count != 0)
            {
                Deselect(Selected.First());
            }
        }

        public static bool TryFind<T>()
        {
            return Selected.Any(selected => selected.TryGetComponent(out T _));
        }
        
        private GameObject GetObjectByMousePosition()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, float.PositiveInfinity, toolMask) &&
                !Physics.Raycast(ray, out hit, float.PositiveInfinity, selectableMask)) return null;
            
            var obj = hit.transform.gameObject;

            if (obj && hit.transform.gameObject.layer != LayerMask.NameToLayer("Tool"))
            {
                return obj;
            }
            else
            {
                return Selected.Last();
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Select();
            }
        }
    }
}