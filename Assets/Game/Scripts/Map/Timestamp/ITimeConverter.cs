namespace Game.Scripts.Map.Timestamp
{
    public interface ITimeConverter
    {
        double ToSecond(MapTime time);

        double ToBeat(MapTime time);
    }
}