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

        private void Start()
        {
            _camera = Camera.main;
        }

        public static OutlinedObject SelectedObj { get; private set; }
        public static OutlinedObject PreviousSelectedObj { get; private set; }

        // ReSharper disable twice Unity.PerformanceCriticalCodeNullComparison
        private void Select()
        {
            if (UIRaycaster.PointerIsOverUI(Input.mousePosition)) return;
            
            var obj = GetObjectByMousePosition();
            
            if (SelectedObj == obj) return;
            if (SelectedObj != null) Deselect();

            Select(obj);
        }

        private void Select(ISelectable[] objects)
        {
            
        }

        public void Select(OutlinedObject obj)
        {
            if ((SelectedObj = obj) == null) return;

            SelectEvent @event = new();
            
            if (obj.TryGetComponent(out ISelectable selectable))
            {
                selectable.OnSelect(@event);
            }
            if (@event.Cancelled) return;

            var selectedObjTransform = SelectedObj.transform;
            var parent = selectedObjTransform.parent = Instantiate(
                emptyObject,
                selectedObjTransform.position,
                selectedObjTransform.rotation
            ).transform;

            parent.name = "Selection";

            SelectedObj.OutlineWidth = 10;
            SelectedObj.OutlineColor = @event.SelectColor;
            mainTools.Activate();
        }

        public void Deselect()
        {
            DeselectEvent @event = new();
            
            if (SelectedObj.TryGetComponent(out ISelectable selectable))
            {
                selectable.OnDeselect(@event);
            }
            if (@event.Cancelled) return;

            Transform parent;
            var selectedObjTransform = SelectedObj.transform;

            selectedObjTransform.parent = (parent = selectedObjTransform.parent).parent;
            Destroy(parent.gameObject);

            SelectedObj.OutlineWidth = 0;
            mainTools.Deactivate();
            
            PreviousSelectedObj = SelectedObj;
            SelectedObj = null;
        }

        private OutlinedObject GetObjectByMousePosition()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 100, toolMask) &&
                !Physics.Raycast(ray, out hit, 100, selectableMask)) return null;

            var obj = hit.transform.GetComponent<OutlinedObject>();
            return obj ? obj : SelectedObj;
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