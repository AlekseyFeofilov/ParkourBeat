using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Engine.Api.Event;
using Game.Scripts.Engine.Api.Listener;
using Game.Scripts.Engine.Select;
using Libraries.QuickOutline.Scripts;
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
        private readonly Dictionary<Transform, Transform> _childToParentDictionary = new();
        private readonly Dictionary<GameObject, OutlinedObjectData> _objectToOutlineDictonary = new();

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

        public bool Select(GameObject obj)
        {
            if (obj == null || Selected.Contains(obj)) return false;
            
            var @event = SelectRequest(obj);
            if (@event.Cancelled) return false;

            Selected.Add(obj);

            if (obj.TryGetComponent(out OutlinedObject outlined))
            {
                _objectToOutlineDictonary[obj] = new OutlinedObjectData(outlined);
            }
            else
            {
                outlined = obj.AddComponent<OutlinedObject>();
            }
            
            Select(outlined, @event);
            return true;
        }
        
        private void Select(OutlinedObject obj, SelectEvent @event)
        {
            if (Selected.Count == 1)
            {
                _wrapper = AddWrapper();
            }

            Transform objTransform = obj.transform;
            
            // ???????????????????????? ?? ??????????????, ?????????? ?????????? ?????????????? ?? ????????????????
            _childToParentDictionary[objTransform] = objTransform.parent;
            
            objTransform.parent = _wrapper;
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
            ToolManager.Activate();
            return wrapper;
        }

        private void RemoveWrapper()
        {
            var selectedTransform = Selected[0].transform;
            var wrapperTransform = selectedTransform.parent;
            Destroy(wrapperTransform.parent.gameObject);
            Destroy(wrapperTransform.gameObject);
            
            selectedTransform.parent = _childToParentDictionary[selectedTransform];
            _childToParentDictionary.Remove(selectedTransform);
            
            ToolManager.Deactivate();
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
                objTransform.parent = _childToParentDictionary[objTransform];
                _childToParentDictionary.Remove(objTransform);
            }

            Selected.Remove(obj);

            if (_objectToOutlineDictonary.ContainsKey(obj))
            {
                if (obj.TryGetComponent(out OutlinedObject outlinedObject))
                {
                    OutlinedObjectData data = _objectToOutlineDictonary[obj];
                    outlinedObject.OutlineColor = data.Color;
                    outlinedObject.OutlineWidth = data.Width;
                    outlinedObject.OutlineMode = data.Mode;
                    outlinedObject.enabled = true;
                }

                _objectToOutlineDictonary.Remove(obj);
            }
            else
            {
                Destroy(obj.GetComponent<OutlinedObject>());
            }
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
            
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Tool") &&
                Input.GetKey(KeyCode.LeftControl)) return null;
            
            var obj = hit.transform.gameObject;

            if (obj && hit.transform.gameObject.layer != LayerMask.NameToLayer("Tool"))
            {
                return obj;
            }

            return Selected.Last();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Select();
            }
        }
        
        private class OutlinedObjectData
        {
            public readonly Color Color;
            public readonly float Width;
            public readonly OutlinedObject.Mode Mode;

            public OutlinedObjectData(OutlinedObject outlinedObject)
            {
                Color = outlinedObject.OutlineColor;
                Width = outlinedObject.OutlineWidth;
                Mode = outlinedObject.OutlineMode;
            }
        }
    }
}