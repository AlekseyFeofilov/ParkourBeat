using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Engine.Api.Event;
using Game.Scripts.Engine.Api.Listener;
using HSVPicker;
using UnityEngine;

namespace Game.Scripts.Engine.Manager
{
    public class ToolManager : MonoBehaviour
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

        private static bool _needsUpdate;

        [SerializeField] private GameObject moveTool;

        [SerializeField] private GameObject rotateTool;

        [SerializeField] private GameObject scaleTool;

        [SerializeField] private GameObject colorTool;

        [SerializeField] private GameObject canvas;

        private GameObject _currentTool;
        private SelectManager _selectManager;

        private bool _activated;

        private void Start()
        {
            _selectManager = FindObjectOfType<SelectManager>();
        }

        private void Update()
        {
            if (_activated && !Input.GetKey(KeyCode.LeftControl))
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
                    if (SelectManager.TryFind<IMovable>())
                    {
                        AddTool(moveTool);
                    }

                    break;

                case Mode.RotateTool:
                    if (SelectManager.TryFind<IRotatable>())
                    {
                        AddTool(rotateTool);
                    }

                    break;

                case Mode.ScaleTool:
                    if (SelectManager.TryFind<IScalable>())
                    {
                        AddTool(scaleTool);
                    }

                    break;

                case Mode.ColorTool:
                    if (SelectManager.TryFind<IColorable>())
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
            var selectedObjTransform = SelectManager.Selected[0].transform;

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
            var copySelected = new List<GameObject>(SelectManager.Selected);

            foreach (var selected in copySelected.Where(selected =>
                         !selected.TryGetComponent(out IColorable _))
                    )
            {
                _selectManager.Deselect(selected);
            }
        }

        private static void StartColorTool(ColorPicker picker)
        {
            ColorBeginEvent @event = new();
            if (SelectManager.Selected[0].TryGetComponent(out Renderer component))
            {
                @event.StartColor = component.material.color;
            }
            
            if (SelectManager.Selected[0].TryGetComponent(out IColorable colorable))
            {
                colorable.OnBeginColor(@event);
                picker._color = @event.StartColor;
            }
        }

        private static void SetColorToolListener(ColorPicker picker)
        {
            picker.onValueChanged.AddListener(color =>
            {
                foreach (var selected in SelectManager.Selected)
                {
                    if (!selected.TryGetComponent(out IColorable colorable)) continue;
                    if (!colorable.OnChange(color)) continue;
                    if (!selected.TryGetComponent(out Renderer component)) continue;
                
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
            if (SelectManager.Selected.Count == 0) return;
            SelectManager.Selected.First().transform.parent.parent.Translate(direction);
        }

        public static void Rotate(Vector3 rotation)
        {
            if (SelectManager.Selected.Count == 0) return;
            SelectManager.Selected.First().transform.parent.Rotate(rotation);
        }

        public static void Scale(Vector3 scaling)
        {
            if (SelectManager.Selected.Count == 0) return;

            foreach (var selected in SelectManager.Selected)
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
            if (SelectManager.Selected.Count == 0) return;
            SelectManager.Selected.First().transform.parent.position = direction;
        }

        public static void RotationReset(bool resetOx, bool resetOy, bool resetOz)
        {
            if (SelectManager.Selected.Count == 0) return;

            var rotarion1 = SelectManager.Selected.First().transform.parent.rotation;
            var rotarion2 = SelectManager.Selected.First().transform.parent.parent.rotation;

            SelectManager.Selected.First().transform.parent.rotation = Quaternion.Euler(
                resetOx ? 0 : 1 * rotarion1.x,
                resetOx ? 0 : 1 * rotarion1.y,
                resetOx ? 0 : 1 * rotarion1.z
            );

            SelectManager.Selected.First().transform.parent.parent.rotation = Quaternion.Euler(
                resetOx ? 0 : 1 * rotarion2.x,
                resetOx ? 0 : 1 * rotarion2.y,
                resetOx ? 0 : 1 * rotarion2.z
            );
        }

        public static void UpdateTool()
        {
            _needsUpdate = true;
        }

        public static void SetScale(Vector3 scaling)
        {
            if (SelectManager.Selected.Count == 0) return;
            SelectManager.Selected.First().transform.parent.localScale = scaling;
        }
    }
}