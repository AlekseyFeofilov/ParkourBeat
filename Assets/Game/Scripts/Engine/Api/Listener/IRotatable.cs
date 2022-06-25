using UnityEngine;

namespace Game.Scripts.Engine.Api.Listener
{
    public interface IRotatable
    {
        public bool OnBeginRotate() { return true; }
        public bool OnRotate(Vector3 rotation) { return true; }
        public bool OnEndRotate() { return true; }
    }
}