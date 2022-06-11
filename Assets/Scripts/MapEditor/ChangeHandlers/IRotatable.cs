namespace MapEditor.ChangeHandlers
{
    public interface IRotatable : IChangeable
    {
        public void OnBeginRotate();
        public void OnRotate();
        public void OnEndRotate();
    }
}