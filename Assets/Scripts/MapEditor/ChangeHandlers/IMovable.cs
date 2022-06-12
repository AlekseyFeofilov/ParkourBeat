namespace MapEditor.ChangeHandlers
{
    public interface IMovable : IChangeable
    {
        public void OnBeginMove() {}
        public void OnMove() {}
        public void OnEndMove() {}
    }
}