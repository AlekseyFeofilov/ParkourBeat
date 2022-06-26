using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.BiDictionary;
using Game.Scripts.Map.VisualEffect.Object;
using Game.Scripts.Map.VisualEffect.Property;
using Game.Scripts.Serialization.Data;
using UnityEngine;

namespace Game.Scripts.Map.Manager
{
    public class ObjectManager : MonoBehaviour, IStored
    {
        private readonly BiDictionary<long, MonoObject> _objects = new();

        [SerializeField] public SkyObject skyObject;
        [SerializeField] public HorizonObject horizonObject;
        [SerializeField] private Timeline.Timeline timeline;
        [SerializeField] private TriggerManager triggerManager;
        
        [SerializeField] private CubeObject cubePrefab; 
        
        private long _counter;

        public ICollection<MonoObject> Objects => _objects.ValueMap.Keys;

        private void Awake()
        {
            _objects.Add(-1L, skyObject);
            _objects.Add(-2L, horizonObject);
        }

        public MonoObject CreateOrGetObject(string type, long id)
        {
            if (_objects.KeyMap.TryGetValue(id, out MonoObject obj)) return obj;

            obj = (type switch
            {
                "sky" => throw new InvalidOperationException("sky is single object"),
                "horizon" => throw new InvalidOperationException("horizon is single object"),
                "cube" => Instantiate(cubePrefab),
                _ => throw new InvalidOperationException("unsupported type")
            }).GetComponent<MonoObject>();

            obj.transform.parent = transform;
            _objects.Add(id, obj);

            if (_counter < id) _counter = id;
            _counter++;
            
            return obj;
        }

        public MonoObject CopyObject(MonoObject monoObject)
        {
            MonoObject copy = CreateObject(GetTypeByObject(monoObject)); 
            
            foreach (var (id, property) in monoObject.Properties)
            {
                copy.GetPropertyById(id).SetDefault(property.GetDefault());
            }

            return copy;
        }

        public string GetTypeByObject(MonoObject monoObject)
        {
            return monoObject switch
            {
                SkyObject => "sky",
                HorizonObject => "horizon",
                CubeObject => "cube",
                _ => throw new InvalidOperationException("unsupported type")
            };
        }

        public void RemoveObject(MonoObject monoObject)
        {
            Destroy(monoObject.gameObject);
            _objects.RemoveValue(monoObject);
            foreach (var property in monoObject.Properties.ValueMap.Keys)
            {
                timeline.RemoveVisualProperty(property);
            }

            triggerManager.RemoveObject(monoObject);
        }

        public MonoObject CreateObject(string type)
        {
            return CreateOrGetObject(type, _counter);
        }
        
        public long GetIdByObject(MonoObject monoObject)
        {
            return _objects.ValueMap[monoObject];
        }

        public MonoObject GetObjectById(long id)
        {
            return _objects.KeyMap[id];
        }

        public VisualPropertyId GetIdByProperty(IVisualProperty property)
        {
            return new VisualPropertyId
            {
                ObjectId = GetIdByObject(property.Parent),
                PropertyId = property.Parent.GetIdByProperty(property)
            };
        }
        
        public IVisualProperty GetPropertyById(VisualPropertyId id)
        {
            return GetObjectById(id.ObjectId).GetPropertyById(id.PropertyId);
        }

        public void SaveData(BeatmapData data)
        {
            data.Objects.AddRange(from objPair in _objects.KeyMap 
                select new ObjectData
                {
                    Id = objPair.Key,
                    Type = GetTypeByObject(objPair.Value),
                    Properties = (from propPair in objPair.Value.Properties
                        select new VisualPropertyData
                        {
                            Id = propPair.Key,
                            Default = new ValueData(propPair.Value.GetDefault())
                        }).ToList()
                });
        }

        public void LoadData(BeatmapData data)
        {
            foreach (var obj in data.Objects)
            {
                MonoObject monoObject = CreateOrGetObject(obj.Type, obj.Id);

                foreach (var prop in obj.Properties)
                {
                    IVisualProperty visualProperty = monoObject.GetPropertyById(prop.Id);
                    visualProperty.SetDefault(prop.Default.Value);
                }
            }
        }
    }
}