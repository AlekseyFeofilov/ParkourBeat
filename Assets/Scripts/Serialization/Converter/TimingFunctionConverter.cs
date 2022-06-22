using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using VisualEffect.Function;

namespace Serialization.Converter
{
    public class TimingFunctionConverter : JsonConverter<ITimingFunction>
    {
        private const string BezierRegex = @"[\d,]+";  // language=regex
        
        public override void WriteJson(JsonWriter writer, ITimingFunction value, JsonSerializer serializer)
        {
            switch (value)
            {
                case LinearFunction:
                    writer.WriteValue("linear");
                    break;
                case CubicBezierFunction bezier:
                    writer.WriteValue("cubic-bezier(" + bezier.X1 + ";" + bezier.Y1 + ";" + bezier.X2 + ";" + bezier.Y2 + ")");
                    break;
            }
        }

        public override ITimingFunction ReadJson(JsonReader reader, Type objectType, ITimingFunction existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            string read = (string) reader.Value;

            if (read == null)
            {
                return null;
            }
            
            if (read == "linear")
            {
                return ITimingFunction.Linear;
            }

            if (read.StartsWith("cubic-bezier"))
            {
                MatchCollection matches = Regex.Matches(read, BezierRegex);

                double x1 = double.Parse(matches[0].Value);
                double y1 = double.Parse(matches[1].Value);
                double x2 = double.Parse(matches[2].Value);
                double y2 = double.Parse(matches[3].Value);

                return new CubicBezierFunction(x1, y1, x2, y2);
            }

            return null;
        }
    }
}