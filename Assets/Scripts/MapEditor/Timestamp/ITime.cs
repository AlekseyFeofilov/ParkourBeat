using Serialization.Data;

namespace MapEditor.Timestamp
{
    public interface ITime
    {
        public double ToSecond(ITimeConverter converter);
         
        public static BaseTime Zero => new BaseTime(TimeUnit.Second, 0);

        public static BaseTime OfBeat(double beat)
        {
            return new BaseTime(TimeUnit.Beat, beat);
        }
        
        public static BaseTime OfSecond(double second)
        {
            return new BaseTime(TimeUnit.Second, second);
        }
        
        public static TimeSum operator +(ITime first, ITime second)
        {
            return new TimeSum(first, second);
        }
        
        public static TimeSub operator -(ITime first, ITime second)
        {
            return new TimeSub(first, second);
        }
    }
    
    public class BaseTime : ITime 
    {
        public TimeUnit Unit;
        public double Value;

        public BaseTime(TimeUnit unit, double value)
        {
            Unit = unit;
            Value = value;
        }

        public double ToSecond(ITimeConverter converter)
        {
            return converter.ToSecond(this);
        }
    }
    
    public class TimeSum : ITime
    {
        private ITime _first;
        private ITime _second;

        public TimeSum(ITime first, ITime second)
        {
            _first = first;
            _second = second;
        }

        public double ToSecond(ITimeConverter converter)
        {
            if (_first is BaseTime firstBase && _second is BaseTime secondBase && 
                firstBase.Unit == secondBase.Unit)
            {
                return converter.ToSecond(new BaseTime(firstBase.Unit,
                    firstBase.Value + secondBase.Value));
            }
            
            return _first.ToSecond(converter) + _second.ToSecond(converter);
        }
    }
    
    public class TimeSub : ITime
    {
        private ITime _first;
        private ITime _second;

        public TimeSub(ITime first, ITime second)
        {
            _first = first;
            _second = second;
        }

        public double ToSecond(ITimeConverter converter)
        {
            if (_first is BaseTime firstBase && _second is BaseTime secondBase && 
                firstBase.Unit == secondBase.Unit)
            {
                return converter.ToSecond(new BaseTime(firstBase.Unit,
                    firstBase.Value - secondBase.Value));
            }
            
            return _first.ToSecond(converter) - _second.ToSecond(converter);
        }
    }
}