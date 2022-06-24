using System.Collections.Generic;
using System.Linq;
using MapEditor.Tools;
using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace MapEditor.Select
{
    public class MainSelect : MonoBehaviour
    {
        public LayerMask selectableMask;
        public LayerMask toolMask;
        public GameObject emptyObject;

        private Camera _camera;

        [SerializeField] private MainTools mainTools;

        private Transform _wrapper;

        private void Awake()
        {
            Selected = new List<OutlinedObject>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        public static List<OutlinedObject> Selected { get; private set; }

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

        private void Select(ISelectable[] objects)
        {
        }

        public void Select(OutlinedObject obj)
        {
            if (obj == null) return;

            var @event = SelectRequest(obj);
            if (@event.Cancelled) return;

            Selected.Add(obj);
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
            mainTools.Activate();
            return wrapper;
        }

        private void RemoveWrapper()
        {
            Transform parent;
            var selectedTransform = Selected[0].transform;

            selectedTransform.parent = (parent = selectedTransform.parent).parent.parent;
            Destroy(parent.parent.gameObject);
            Destroy(parent.gameObject);
            mainTools.Deactivate();
        }

        private static SelectEvent SelectRequest(OutlinedObject obj)
        {
            SelectEvent @event = new();

            if (obj.TryGetComponent(out ISelectable selectable))
            {
                selectable.OnSelect(@event);
            }

            return @event;
        }

        private static DeselectEvent DeselectRequest(OutlinedObject obj)
        {
            DeselectEvent @event = new();

            if (obj.TryGetComponent(out ISelectable selectable))
            {
                selectable.OnDeselect(@event);
            }

            return @event;
        }

        public void Deselect(OutlinedObject obj)
        {
            var @event = DeselectRequest(obj);
            if (@event.Cancelled) return;
            if(obj == Selected[0]) mainTools.Hide();
            
            if (Selected.Count == 1)
            {
                RemoveWrapper();
            }
            else
            {
                var objTransform = obj.transform;
                objTransform.parent = objTransform.parent.parent.parent;
            }

            obj.OutlineWidth = 0;
            Selected.Remove(obj);
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
        
        private OutlinedObject GetObjectByMousePosition()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 100, toolMask) &&
                !Physics.Raycast(ray, out hit, 100, selectableMask)) return null;

            var obj = hit.transform.GetComponent<OutlinedObject>();
            return obj ? obj : Selected.Last();
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