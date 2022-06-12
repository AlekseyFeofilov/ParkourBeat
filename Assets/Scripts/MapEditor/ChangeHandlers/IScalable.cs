namespace MapEditor.ChangeHandlers
{
    public interface IScalable : IChangeable
    {
        public void OnBeginScale() {}
        public void OnScale() {}
        public void OnEndScale() {}
    }
}