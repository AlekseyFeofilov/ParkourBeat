namespace MapEditor.Utils
{
    public interface IMovable
    {
        public void OnBeginMove() {}

        public void OnMove() {}
        
        public void OnEndMove() {}
    }
}