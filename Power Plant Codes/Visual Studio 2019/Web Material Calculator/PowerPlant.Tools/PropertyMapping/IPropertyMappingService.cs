using System.Collections.Generic;

namespace PowerPlant.Tools.PropertyMapping
{
    public interface IPropertyMappingService
    {
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();

        public bool IsMappingValidated<TSource, TDestination>(string fields);

        public bool IsPropertiesValidated<TSource>(string fields);
    }
}