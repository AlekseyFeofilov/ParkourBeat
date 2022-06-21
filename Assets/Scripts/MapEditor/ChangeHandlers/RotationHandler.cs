using MapEditor.Select;

namespace MapEditor.ChangeHandlers
{
    public class RotationHandler : ChangesHandler
    {
        private IRotatable _rotatable;

        protected override void OnBeginChange() => _rotatable.OnBeginRotate();

        protected override void OnChange() => _rotatable.OnRotate();

        protected override void OnEndChange() => _rotatable.OnEndRotate();

        protected override IChangeable IsChangeable() =>
            _rotatable = MainSelect.SelectedObj.transform.GetComponent<IRotatable>();
    }
}