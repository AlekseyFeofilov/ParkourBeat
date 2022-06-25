using UnityEngine;

namespace Game.Scripts.Engine.Tool
{
    public interface ITool
    {
        protected internal void Change(Vector3 change);

        protected internal Vector3 GetChange(float speed, string tag)
        {
            return tag switch
            {
                "OX" => ChangeOx(speed),
                "OY" => ChangeOy(speed),
                "OZ" => ChangeOz(speed),
                _ => Vector3.zero
            };
        }
        
        protected Vector3 ChangeOx(float speed)
        {
            return speed * Vector3.right;
        }

        protected Vector3 ChangeOy(float speed)
        {
            return speed * Vector3.up;
        }

        protected Vector3 ChangeOz(float speed)
        {
            return speed * Vector3.forward;
        }
    }
}