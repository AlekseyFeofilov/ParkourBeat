namespace MapEditor.Tools
{
    public interface ITool
    {
        protected internal void Change(float speed, string tag)
        {
            switch (tag)
            {
                case "OX":
                    ChangeOx(speed);
                    break;

                case "OY":
                    ChangeOy(speed);
                    break;

                case "OZ":
                    ChangeOz(speed);
                    break;
            }
        }
        
        protected void ChangeOx(float speed);
        protected void ChangeOy(float speed);
        protected void ChangeOz(float speed);
    }
}