using System;
using MapEditor.Select;
using UnityEngine;

namespace MapEditor
{
    public class MainTools : MonoBehaviour
    {
        private enum Mode
        {
            MoveTool,
            RotateTool,
            ScaleTool,
        }

        private Mode ToolMode
        {
            set
            {
                _toolMode = value;
                _needsUpdate = true;
            }
        }

        private Mode _toolMode = Mode.MoveTool;

        private bool _needsUpdate;

        [SerializeField] private GameObject moveTool;

        [SerializeField] private GameObject rotateTool;

        [SerializeField] private GameObject scaleTool;

        private GameObject _currentTool;

        private bool _activated;

        private void Update()
        {
            if (_activated)
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
            }

            if (!_needsUpdate) return;

            DestroyTool();
            UpdateTools();
            _needsUpdate = false;
        }

        private void UpdateTools()
        {
            // ReSharper disable once Unity.PerformanceCriticalCodeNullComparison
            if (!_activated)
            {
                DestroyTool();
                return;
            }

            switch (_toolMode)
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
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Activate()
        {
            _activated = true;
            _needsUpdate = true;
        }

        public void Deactivate()
        {
            _activated = false;
            _needsUpdate = true;
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

        public static void SetPosition(Vector3 direction)
        {
            if (!MainSelect.SelectedObj) return;
            MainSelect.SelectedObj.transform.parent.position = direction;
        }

        public static void SetRotation(Quaternion rotation)
        {
            if (!MainSelect.SelectedObj) return;
            MainSelect.SelectedObj.transform.parent.rotation = rotation;
        }

        public static void SetScale(Vector3 scaling)
        {
            if (!MainSelect.SelectedObj) return;
            MainSelect.SelectedObj.transform.localScale = scaling;
        }
    }
}