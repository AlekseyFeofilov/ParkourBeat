namespace MapEditor.Timestamp
{
    public interface ITimeConverter
    {
        double ToSecond(MapTime time);

        double ToBeat(MapTime time);
    }
}