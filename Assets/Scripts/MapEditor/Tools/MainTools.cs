using System;
using System.Collections.Generic;
using System.Linq;
using HSVPicker;
using MapEditor.ChangeableInterfaces;
using MapEditor.Select;
using UnityEngine;

namespace MapEditor.Tools
{
    public class MainTools : MonoBehaviour
    {
        private enum Mode
        {
            MoveTool,
            RotateTool,
            ScaleTool,
            ColorTool,
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

        [SerializeField] private GameObject colorTool;

        [SerializeField] private GameObject canvas;

        private GameObject _currentTool;
        private MainSelect _mainSelect;

        private bool _activated;

        private void Start()
        {
            _mainSelect = FindObjectOfType<MainSelect>();
        }

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
                    if (MainSelect.TryFind<IMovable>())
                    {
                        AddTool(moveTool);
                    }

                    break;

                case Mode.RotateTool:
                    if (MainSelect.TryFind<IRotatable>())
                    {
                        AddTool(rotateTool);
                    }

                    break;

                case Mode.ScaleTool:
                    if (MainSelect.TryFind<IScalable>())
                    {
                        AddTool(scaleTool);
                    }

                    break;

                case Mode.ColorTool:
                    if (MainSelect.TryFind<IColorable>())
                    {
                        AddColorTool();
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Hide()
        {
            DestroyTool();
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
            var selectedObjTransform = MainSelect.Selected[0].transform;

            _currentTool = Instantiate(
                tool,
                selectedObjTransform.position,
                selectedObjTransform.rotation,
                selectedObjTransform.parent.parent
            );
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void AddColorTool()
        {
            _currentTool = Instantiate(colorTool, canvas.transform);
            var picker = _currentTool.GetComponent<ColorPicker>();
            ColorToolCleaner();
            StartColorTool(picker);
            SetColorToolListener(picker);
        }

        private void ColorToolCleaner()
        {
            var copySelected = new List<OutlinedObject>(MainSelect.Selected);

            foreach (var selected in copySelected.Where(selected =>
                         !selected.TryGetComponent(out IColorable _) ||
                         !selected.TryGetComponent(out Renderer _))
                    )
            {
                _mainSelect.Deselect(selected);
            }
        }

        private static void StartColorTool(ColorPicker picker)
        {
            if (MainSelect.Selected[0].TryGetComponent(out Renderer component))
            {
                picker._color = component.material.color;
            }
        }

        private static void SetColorToolListener(ColorPicker picker)
        {
            picker.onValueChanged.AddListener(color =>
            {
                foreach (var component in
                         from selected in MainSelect.Selected
                         let component = selected.GetComponent<Renderer>()
                         let colorable = selected.GetComponent<IColorable>()
                         where colorable.OnChange(color)
                         select component)
                {
                    component.material.color = color;
                }
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
            if (MainSelect.Selected.Count == 0) return;
            MainSelect.Selected.First().transform.parent.parent.Translate(direction);
        }

        public static void Rotate(Vector3 rotation)
        {
            if (MainSelect.Selected.Count == 0) return;
            MainSelect.Selected.First().transform.parent.Rotate(rotation);
        }

        public static void Scale(Vector3 scaling)
        {
            if (MainSelect.Selected.Count == 0) return;

            foreach (var selected in MainSelect.Selected)
            {
                var localScale = selected.transform.localScale;
                if (
                    localScale.x + scaling.x < 0 ||
                    localScale.y + scaling.y < 0 ||
                    localScale.z + scaling.z < 0
                ) return;

                selected.transform.localScale += scaling;
            }
        }

        public static void SetPosition(Vector3 direction)
        {
            if (MainSelect.Selected.Count == 0) return;
            MainSelect.Selected.First().transform.parent.position = direction;
        }

        public static void SetRotation(Quaternion rotation)
        {
            if (MainSelect.Selected.Count == 0) return;
            MainSelect.Selected.First().transform.parent.rotation = rotation;
        }

        public static void SetScale(Vector3 scaling)
        {
            if (MainSelect.Selected.Count == 0) return;
            MainSelect.Selected.First().transform.parent.localScale = scaling;
        }
    }
}