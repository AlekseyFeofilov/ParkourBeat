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
        
        public Mode ToolMode {
            get => toolMode;
            set {
                toolMode = value;
                _needsUpdate = true;
            }
        }
        
        [SerializeField]
        private Mode toolMode = Mode.None;
        
        private bool _needsUpdate;
        
        [SerializeField]
        private GameObject moveTool;
        
        [SerializeField]
        private GameObject rotateTool;
        
        [SerializeField]
        private GameObject scaleTool;

        private GameObject _currentTool;
        
        private void Update()
        {
            if (!_needsUpdate) return;
            
            DestroyTool();
            _needsUpdate = false;
            UpdateTools();
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
            var transform1 = MainSelect.SelectedObj.transform;
            _currentTool = Instantiate(tool, transform1.position, transform1.rotation, transform1);
        }

        private void DestroyTool()
        {
            if(!_currentTool) return;
            Destroy(_currentTool);
        }

        public static void Move(Vector3 direction)
        {
            if(!MainSelect.SelectedObj) return;
            MainSelect.SelectedObj.transform.Translate(direction);
        }

        public static void Rotate(Vector3 direction)
        {
            if(!MainSelect.SelectedObj) return;
            MainSelect.SelectedObj.transform.Rotate(direction);
        }

        public static void Scale(Vector3 direction)
        {
            if(!MainSelect.SelectedObj) return;
            MainSelect.SelectedObj.transform.localScale += direction;
        }
    }
}
