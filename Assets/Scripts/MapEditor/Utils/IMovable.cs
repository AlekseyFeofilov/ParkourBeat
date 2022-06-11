namespace MapEditor.Utils
{
    public interface IMovable
    {
        public bool OnBeginMove()
        {
            return true;
        }

        public bool OnMove()
        {
            return true;
        }

        public bool OnEndMove()
        {
            return true;
        }
    }
}