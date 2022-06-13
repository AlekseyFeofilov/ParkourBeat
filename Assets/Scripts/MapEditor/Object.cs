using UnityEngine;

namespace MapEditor
{
    public class ObjectMap
    {
        private GameObject _object;
        private GameObject _settings;
        GameObject TargetObj;
        private Object _actionTarget;

        public ObjectMap(GameObject obj, GameObject settings)
        {
            _object = obj;
            _settings = settings;
        }

        private string Title;
        //мб понадобиться
        // private float _size;
        // private float[] _rotate = new float[3];
        // private float[] _scale = new float[3];
        // private bool _decoration = false;
    }
}