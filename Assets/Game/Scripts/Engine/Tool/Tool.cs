using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Engine.Manager;
using Game.Scripts.Engine.Visual;
using UnityEngine;

namespace Game.Scripts.Engine.Tool
{
    public abstract class Tool : ActivatingObject
    {
        protected readonly Dictionary<GameObject, Transform> Data = new(); 
        
        [SerializeField] private float speedBooster = 1f;
        [SerializeField] protected float rotateSpeed = 1f;

        protected SelectManager SelectManager;
        private ToolManager _toolManager;
        private Camera _camera;

        protected float Speed;
        protected float ScaledSpeed;

        private float _lenght;
        private Vector3 _projection;
        private Vector3 _offset;

            protected override void Start()
        {
            base.Start();
            _camera = Camera.main;
            SelectManager = FindObjectOfType<SelectManager>();
            _toolManager = FindObjectOfType<ToolManager>();
        }

        protected override void OnMouseDown()
        {
            base.OnMouseDown();

            foreach (var selected in SelectManager.Selected.Where(OnBegin))
            {
                AddChangeHandler(selected);
                Data[selected] = selected.transform;
                _offset = Input.mousePosition;
                CalculateProperties();
            }

            var copySelected =new List<GameObject>(SelectManager.Selected);
            
            foreach (var selected in copySelected.Where(selected => !Data.ContainsKey(selected)))
            {
                SelectManager.Deselect(selected);
            }
        }

        protected override void OnMouseUp()
        {
            base.OnMouseUp();

            while (Data.Count != 0)
            {
                EndChange(Data.First());
            }
            
            ToolManager.UpdateTool();
        }

        private void EndChange(KeyValuePair<GameObject, Transform> data)
        {
            if (!OnEnd(data.Key))
            {
                var selectedObjTransform = data.Key.transform;
                selectedObjTransform.position = data.Value.position;
                selectedObjTransform.rotation = data.Value.rotation;
                selectedObjTransform.localScale = data.Value.localScale;
                
                _toolManager.Hide();
            }

            Data.Remove(data.Key);
        }

        protected virtual void Update()
        {
            if (Data.Count != 0)
            {
                CalculateProperties();
                CalculateChanges();
            }
            
            foreach (var data in Data)
            {
                if (Vector3.Distance(data.Key.transform.position, data.Value.position) > 100)
                {
                    EndChange(data);
                    continue;
                }

                ChangeRequest(data);
            }
            
            if(Data.Count != 0) Change();
        }
        
        private void CalculateProperties()
        {
            var end = transform.GetChild(1).transform.position;
            var begin = transform.GetChild(0).transform.position;
            _lenght = (end - begin).magnitude;

            var endProjection = _camera.WorldToScreenPoint(end);
            var beginProjection = _camera.WorldToScreenPoint(begin);
            _projection = endProjection - beginProjection;
            _projection.z = 0;
        }

        private void CalculateChanges()
        {
            var movement = Input.mousePosition - _offset;
            var projection = Vector3.Project(movement, _projection);

            var sign = Vector3.Angle(projection, _projection) < 90 ? 1 : -1;

            Speed = projection.magnitude * sign;
            ScaledSpeed = Speed * _lenght * speedBooster / _projection.magnitude;

            _offset = Input.mousePosition;
        }

        protected abstract void AddChangeHandler(GameObject selected);
        
        protected abstract bool OnBegin(GameObject selected);

        protected abstract void Change();

        protected abstract void ChangeRequest(KeyValuePair<GameObject, Transform> data);

        protected abstract bool OnEnd(GameObject selected);
    }
}