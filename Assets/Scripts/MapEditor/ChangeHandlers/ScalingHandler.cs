using MapEditor.Select;

namespace MapEditor.ChangeHandlers
{
    public class ScalingHandler : ChangesHandler
    {
        private IScalable _scalable;

        protected override void OnBeginChange() => _scalable.OnBeginScale();

        protected override void OnChange() => _scalable.OnScale();

        protected override void OnEndChange() => _scalable.OnEndScale();

        protected override IChangeable IsChangeable() =>
            _scalable = MainSelect.SelectedObj.transform.GetComponent<IScalable>();
    }
}