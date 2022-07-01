using Game.Scripts.Engine.Api.Event;
using UnityEngine;

namespace Game.Scripts.Engine.Api.Listener
{
    public interface IColorable
    {
        public bool OnChange(Color color) { return true; }

        public void OnBeginColor(ColorBeginEvent @event)
        {
        }
    }
}