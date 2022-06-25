using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Game.Scripts.Serialization
{
    public class JsonManager
    {
        private JsonSerializer _serializer = new();

        public Formatting Formatting
        {
            get => _serializer.Formatting;
            set => _serializer.Formatting = value;
        }
        
        /// <summary>
        /// Метод добавления конвертера
        /// </summary>
        /// <param name="converter">Json конвертер</param>
        public void AddConverter(JsonConverter converter)
        {
            _serializer.Converters.Add(converter);
        }
        
        /// <summary>
        /// Метод сериализации
        /// </summary>
        /// <param name="value">Объект, который нужно сериализовать</param>
        /// <returns>Сериализованная json строка</returns>
        public string Serialize(object value)
        {
            StringWriter stringWriter = new(new StringBuilder(256));
            using (JsonTextWriter jsonTextWriter = new(stringWriter))
            {
                jsonTextWriter.Formatting = Formatting;
                _serializer.Serialize(jsonTextWriter, value);
            }
            return stringWriter.ToString();
        }
    
        /// <summary>
        /// Метод десериализации
        /// </summary>
        /// <param name="json">Json строка, которую нужно десериализовать</param>
        /// <typeparam name="T">Тип объекта, который нужно десериализовать</typeparam>
        /// <returns>Десериализованный объект</returns>
        public T Deserialize<T>(string json)
        {
            using JsonTextReader jsonTextReader = new JsonTextReader(new StringReader(json));
            return (T) _serializer.Deserialize(jsonTextReader, typeof (T));
        }
    }
}