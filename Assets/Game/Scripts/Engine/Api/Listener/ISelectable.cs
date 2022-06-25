using Game.Scripts.Engine.Api.Event;

namespace Game.Scripts.Engine.Api.Listener
{
    public interface ISelectable
    {
        public virtual void OnSelect(SelectEvent @event) { }

        public void OnDeselect(DeselectEvent @event) { }
    }
}