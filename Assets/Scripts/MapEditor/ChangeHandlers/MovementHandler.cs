using MapEditor.Select;

namespace MapEditor.ChangeHandlers
{
    public class MovementHandler : ChangesHandler
    {
        private IMovable _movable;

        protected override void OnBeginChange() => _movable.OnBeginMove();

        protected override void OnChange() => _movable.OnMove();

        protected override void OnEndChange() => _movable.OnEndMove();

        protected override IChangeable IsChangeable() =>
            _movable = MainSelect.SelectedObj.transform.GetComponent<IMovable>();
    }
}