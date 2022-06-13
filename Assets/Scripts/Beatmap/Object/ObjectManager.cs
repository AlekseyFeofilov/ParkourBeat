using System;
using System.Linq;
using DataStructures.BiDictionary;
using MapEditor;
using Serialization.Data;
using UnityEngine;
using VisualEffect.Object;
using VisualEffect.Property;

namespace Beatmap.Object
{
    public class ObjectManager : MonoBehaviour, IStored
    {
        private readonly BiDictionary<long, MonoObject> _objects = new();

        [SerializeField] public SkyObject skyObject;
        
        [SerializeField] private CubeObject cubePrefab; 
        
        private long _counter;

        private void Awake()
        {
            _objects.Add(-1L, skyObject);
        }

        public MonoObject CreateOrGetObject(string type, long id)
        {
            if (_objects.KeyMap.TryGetValue(id, out MonoObject obj)) return obj;

            obj = (type switch
            {
                "sky" => throw new InvalidOperationException("sky is single object"),
                "cube" => Instantiate(cubePrefab, new Vector3(0, 0, 0), Quaternion.identity),
                _ => throw new InvalidOperationException("unsupported type")
            }).GetComponent<MonoObject>();

            _objects.Add(id, obj);
            return obj;
        }

        public string GetTypeByObject(MonoObject monoObject)
        {
            return monoObject switch
            {
                SkyObject => "sky",
                CubeObject => "cube",
                _ => throw new InvalidOperationException("unsupported type")
            };
        }

        public MonoObject CreateObject(string type)
        {
            return CreateOrGetObject(type, _counter++);
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