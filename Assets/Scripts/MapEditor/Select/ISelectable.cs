namespace MapEditor.Select
{
    public interface ISelectable
    {
        public virtual void OnSelect(SelectEvent @event) { }

        public void OnDeselect(DeselectEvent @event) { }
    }
}