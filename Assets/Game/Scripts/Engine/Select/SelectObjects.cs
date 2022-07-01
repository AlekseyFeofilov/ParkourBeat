using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Engine.Manager;
using UnityEngine;

namespace Game.Scripts.Engine.Select
{
    public class SelectObjects : MonoBehaviour
    {
        [SerializeField] private SelectManager selectManager;
        private List<GameObject> _selectables;
        private List<GameObject> _selected;

        [SerializeField] private GUISkin skin;
        private Rect _rect;
        private bool _selecting;

        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private Camera _camera;
        
        private void Awake()
        {
            _selectables = new List<GameObject>();
            _selected = new List<GameObject>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) &&
                Input.GetKey(KeyCode.LeftAlt))
            {
                // ReSharper disable two Unity.PerformanceCriticalCodeInvocation
                UpdateAvailableObjects();

                if (!Input.GetKey(KeyCode.LeftControl))
                {
                    selectManager.Deselect();
                }
                
                _selected.Clear();

                _startPosition = Input.mousePosition;
                _selecting = true;
            }

            if (!Input.GetMouseButtonUp(0)) return;
            LocalOnMouseUp();
        }

        private void LocalOnMouseUp()
        {
            _selecting = false;
        }

        private void OnGUI()
        {
            GUI.skin = skin;
            GUI.depth = 99;

            if (!_selecting) return;

            if (!Input.GetKey(KeyCode.LeftAlt))
            {
                LocalOnMouseUp();
            }

            _endPosition = Input.mousePosition;

            if (_startPosition == _endPosition) return;

            _rect = new Rect(Math.Min(_endPosition.x, _startPosition.x),
                Screen.height - Math.Max(_endPosition.y, _startPosition.y),
                Math.Max(_endPosition.x, _startPosition.x) - Math.Min(_endPosition.x, _startPosition.x),
                Math.Max(_endPosition.y, _startPosition.y) - Math.Min(_endPosition.y, _startPosition.y)
            );

            GUI.Box(_rect, "");

            var selected = new List<GameObject>(_selected);
            foreach (var obj in from obj in selected
                     let position = obj.transform.position 
                     let screenPoint = _camera.WorldToScreenPoint(position) 
                     let screenPosition = new Vector2(screenPoint.x, Screen.height - screenPoint.y) 
                     where !_rect.Contains(screenPosition) select obj)
            {
                selectManager.Deselect(obj);
                _selected.Remove(obj);
            }
            
            foreach (var obj in from obj in _selectables
                     let position = obj.transform.position
                     let screenPoint = _camera.WorldToScreenPoint(position)
                     let screenPosition = new Vector2(screenPoint.x, Screen.height - screenPoint.y)
                     where _rect.Contains(screenPosition) select obj)
            {
                if (_selected.Contains(obj)) continue;
                if(selectManager.Select(obj)) _selected.Add(obj);
            }
        }

        private void UpdateAvailableObjects()
        {
            _selectables.Clear();
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            var gameObjects = FindObjectsOfType<GameObject>();

            foreach (var obj in gameObjects)
            {
                if (obj.layer == LayerMask.NameToLayer("Selectable Mask"))
                {
                    _selectables.Add(obj);
                }
            }
        }
    }
}