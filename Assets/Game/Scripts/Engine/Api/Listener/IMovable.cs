using UnityEngine;

namespace Game.Scripts.Engine.Api.Listener
{
    public interface IMovable
    {
        public bool OnBeginMove() { return true; }

        public bool OnMove(Vector3 movement) { return true; }

        public bool OnEndMove() { return true; }
    }
}