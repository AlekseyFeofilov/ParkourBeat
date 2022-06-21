using UnityEngine;

namespace MapEditor.ChangeableInterfaces
{
    public interface IScalable
    {
        public bool OnBeginScale() { return true; }
        public bool OnScale(Vector3 scaling) { return true; }
        public bool OnEndScale() { return true; }
    }
}