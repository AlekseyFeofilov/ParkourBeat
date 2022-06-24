using UnityEngine;

namespace MapEditor.ChangeableInterfaces
{
    public interface IMovable
    {
        public bool OnBeginMove() { return true; }

        public bool OnMove(Vector3 movement) { return true; }

        public bool OnEndMove() { return true; }
    }
}