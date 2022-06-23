using System;
using HSVPicker;
using MapEditor.ChangeableInterfaces;
using MapEditor.Event;
using MapEditor.Select;
using UnityEngine;

namespace MapEditor.Tools
{
    public class MainTools : MonoBehaviour
    {
        public enum Mode
        {
            MoveTool,
            RotateTool,
            ScaleTool,
            ColorTool,
        }

        public Mode ToolMode
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

        [SerializeField] private GameObject colorTool;

        [SerializeField] private GameObject canvas;

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

                if (Input.GetKeyDown(KeyCode.V))
                {
                    ToolMode = Mode.ColorTool;
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
                    if (MainSelect.SelectedObj.TryGetComponent(out IMovable _))
                    {
                        AddTool(moveTool);
                    }
                    break;

                case Mode.RotateTool:
                    if (MainSelect.SelectedObj.TryGetComponent(out IRotatable _))
                    {
                        AddTool(rotateTool);
                    }
                    break;

                case Mode.ScaleTool:
                    if (MainSelect.SelectedObj.TryGetComponent(out IScalable _))
                    {
                        AddTool(scaleTool);
                    }
                    break;

                case Mode.ColorTool:
                    if (MainSelect.SelectedObj.TryGetComponent(out IColorable _))
                    {
                        AddColorTool();
                    }
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

        // ReSharper disable Unity.PerformanceAnalysis
        private void AddColorTool()
        {
            _currentTool = Instantiate(colorTool, canvas.transform);
            var picker = _currentTool.GetComponent<ColorPicker>();
            StartColorTool(picker);
            SetColorToolListener(picker);
        }

        private static void StartColorTool(ColorPicker picker)
        {
            ColorBeginEvent @event = new();
            if (MainSelect.SelectedObj.TryGetComponent(out Renderer component))
            {
                @event.StartColor = component.material.color;
            }
            
            if (MainSelect.SelectedObj.TryGetComponent(out IColorable colorable))
            {
                colorable.OnBeginColor(@event);
                picker._color = @event.StartColor;
            }
        }

        private static void SetColorToolListener(ColorPicker picker)
        {
            picker.onValueChanged.AddListener(color =>
            {
                if (!MainSelect.SelectedObj.TryGetComponent(out IColorable colorable)) return;
                if (!colorable.OnChange(color)) return;
                if (!MainSelect.SelectedObj.TryGetComponent(out Renderer component)) return;
                
                component.material.color = color;
            });
        }

        private void DestroyTool()
        {
            if (!_currentTool) return;
            if (_currentTool == colorTool) DestroyColorTool();

            Destroy(_currentTool);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void DestroyColorTool()
        {
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

            var localScale = MainSelect.SelectedObj.transform.localScale;
            if (
                localScale.x + scaling.x < 0 ||
                localScale.y + scaling.y < 0 ||
                localScale.z + scaling.z < 0
            ) return;
            
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