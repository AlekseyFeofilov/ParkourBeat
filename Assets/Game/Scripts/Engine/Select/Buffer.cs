using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Engine.Select
{
    public class Buffer
    {
        public readonly Dictionary<GameObject, Vector3> Objects;
        public readonly Vector3 Center;

        public Buffer(Dictionary<GameObject, Vector3> objects, Vector3 center)
        {
            Objects = objects;
            Center = center;
        }
    }
}