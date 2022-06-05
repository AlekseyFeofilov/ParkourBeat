namespace MapEditor.Timestamp
{
    public interface ITimeConverter
    {
        double ToSecond(BaseTime time);

        double ToBeat(BaseTime time);
    }
}