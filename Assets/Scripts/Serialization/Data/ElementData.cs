using System;

namespace Serialization.Data
{
    [Serializable]
    public class ElementData
    {
        // Идентификатор элемента
        public long Id;
        
        // Тип элемента (куб, небо и т.д.)
        public string Type;
    }
}