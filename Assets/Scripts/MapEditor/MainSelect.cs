using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

namespace MapEditor
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

        // ReSharper disable twice Unity.PerformanceCriticalCodeNullComparison
        private void Select()
        {
            var obj = GetObjectByMousePosition();
            if (SelectedObj == obj) return;

            if (SelectedObj != null)
            {
                Deselect();
            }

            Select(obj);
        }

        public void Select(OutlinedObject obj)
        {
            if ((SelectedObj = obj) == null) return;

            var selectedObjTransform = SelectedObj.transform;

            var parent = selectedObjTransform.parent = Instantiate(
                emptyObject,
                selectedObjTransform.position,
                selectedObjTransform.rotation
            ).transform;

            parent.name = "Selection";

            SelectedObj.OutlineWidth = 10;
            mainTools.Activate();
        }

        public void Deselect()
        {
            Transform parent;
            var selectedObjTransform = SelectedObj.transform;

            selectedObjTransform.parent = (parent = selectedObjTransform.parent).parent;
            Destroy(parent.gameObject);

            SelectedObj.OutlineWidth = 0;
            mainTools.Deactivate();
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