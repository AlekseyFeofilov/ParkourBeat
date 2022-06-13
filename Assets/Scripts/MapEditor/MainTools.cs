using System;
using UnityEngine;

namespace MapEditor
{
    public class MainTools : MonoBehaviour
    {
        public enum Mode
        {
            MoveTool,
            RotateTool,
            ScaleTool,
            None
        }

        public Mode ToolMode
        {
            get => toolMode;
            set
            {
                toolMode = value;
                _needsUpdate = true;
            }
        }

        [SerializeField] private Mode toolMode = Mode.None;

        private bool _needsUpdate;

        [SerializeField] private GameObject moveTool;

        [SerializeField] private GameObject rotateTool;

        [SerializeField] private GameObject scaleTool;

        private GameObject _currentTool;

        private static Vector3 _position;
        private static Quaternion _rotation;
        private static Vector3 _scale;

        private void Update()
        {
            if (toolMode != Mode.None)
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    ToolMode = Mode.MoveTool;
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    ToolMode = Mode.RotateTool;
                }

                if (Input.GetKeyDown(KeyCode.C))
                {
                    ToolMode = Mode.ScaleTool;
                }

                if (Input.GetKeyDown(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl))
                {
                    RollBack();
                }
            }

            if (!_needsUpdate) return;

            DestroyTool();
            UpdateTools();
            _needsUpdate = false;
        }

        private void UpdateTools()
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
            if (!MainSelect.SelectedObj && toolMode != Mode.None) return;

            switch (toolMode)
            {
                case Mode.MoveTool:
                    AddTool(moveTool);
                    break;

                case Mode.RotateTool:
                    AddTool(rotateTool);
                    break;

                case Mode.ScaleTool:
                    AddTool(scaleTool);
                    break;

                case Mode.None:
                    DestroyTool();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void AddTool(GameObject tool)
        {
            var selectedObjTransform = MainSelect.SelectedObj.transform;

            _currentTool = Instantiate(
                tool,
                selectedObjTransform.position,
                selectedObjTransform.rotation,
                selectedObjTransform.parent
            );
        }

        private void DestroyTool()
        {
            if (!_currentTool) return;
            Destroy(_currentTool);
        }

        public static void Move(Vector3 direction)
        {
            if (!MainSelect.SelectedObj) return;
            MainSelect.SelectedObj.transform.parent.Translate(direction);
        }

        public static void Rotate(Vector3 rotation)
        {
            if (!MainSelect.SelectedObj) return;
            MainSelect.SelectedObj.transform.parent.Rotate(rotation);
        }

        public static void Scale(Vector3 scaling)
        {
            if (!MainSelect.SelectedObj) return;
            MainSelect.SelectedObj.transform.localScale += scaling;
        }

        public static void Save()
        {
            if (!MainSelect.SelectedObj) return;
            var obj = MainSelect.SelectedObj.transform.parent;
            _position = obj.position;
            _rotation = obj.rotation;
            _scale = obj.localScale;
        }
        
        private static void RollBack()
        {
            if (!MainSelect.SelectedObj) return;
            var obj = MainSelect.SelectedObj.transform.parent;
            obj.position = _position;
            obj.rotation = _rotation;
            obj.localScale = _scale;
        }
    }
}