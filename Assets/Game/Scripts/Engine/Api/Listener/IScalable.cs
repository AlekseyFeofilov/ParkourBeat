using UnityEngine;

namespace Game.Scripts.Engine.Api.Listener
{
    public interface IScalable
    {
        public bool OnBeginScale() { return true; }
        public bool OnScale(Vector3 scaling) { return true; }
        public bool OnEndScale() { return true; }
    }
}