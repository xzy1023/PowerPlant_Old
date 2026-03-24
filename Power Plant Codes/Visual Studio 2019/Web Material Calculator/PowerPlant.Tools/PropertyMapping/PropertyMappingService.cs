using PowerPlant.Dtos;
using PowerPlant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PowerPlant.Tools.PropertyMapping
{

    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _webMaterialPropertyMapping =
            new(StringComparer.OrdinalIgnoreCase)
            {
                { "ItemNumber", new PropertyMappingValue(new List<string>() { "ItemNumber" }) },
                { "ItemDescription", new PropertyMappingValue(new List<string>() { "ItemMaster.ItemDesc1 + ItemMaster.ItemDesc2" }) },
            };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<WebMaterialDto, WebMaterial>(_webMaterialPropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            // Get the matching mapping object
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();
            if (matchingMapping.Count() == 1) return matchingMapping.First().MappingDictionary;
            throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}>");
        }

        public bool IsMappingValidated<TSource, TDestination>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields)) return true;

            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();
                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var orderParameter = indexOfFirstSpace == -1 ? null : trimmedField.Substring(indexOfFirstSpace + 1);

                // validate "desc" string
                if (orderParameter is not null && !orderParameter.Equals("desc", StringComparison.CurrentCultureIgnoreCase)) return false;

                // validate field name is in the mapping dictionary
                var propertyName = indexOfFirstSpace == -1 ? trimmedField : trimmedField.Remove(indexOfFirstSpace);
                var propertyMapping = GetPropertyMapping<TSource, TDestination>();
                if (!propertyMapping.ContainsKey(propertyName)) return false;
            }
            return true;
        }

        public bool IsPropertiesValidated<TSource>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields)) return true;

            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo is null) return false;
            }
            return true;
        }
    }
}