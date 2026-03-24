using PowerPlant.Tools.PropertyMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace PowerPlant.Tools.Extensions
{
    
    public static class IQueryableExtensions
    {
        // orderBy string pattern: "dtoPropertyName" or "dtoPropertyName desc" or "dtoPropertyName, dtoPropertyName desc"
        /// <summary>
        /// IQueryable Extensions ApplySort with Dto-Model Property Name Mapping
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <param name="mappingDictionary"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source is null) throw new ArgumentNullException("source");
            if (mappingDictionary is null) throw new ArgumentNullException("mappingDictionary");
            if (string.IsNullOrEmpty(orderBy)) return source;

            var orderByString = string.Empty;
            var orderByAfterSplit = orderBy.Split(',');
            foreach (var orderByStr in orderByAfterSplit)
            {
                // Remove spaces
                var trimmedorderByStr = orderByStr.Trim();
                // Determine sort direction
                var isOrderByDescending = trimmedorderByStr.EndsWith("desc");
                // Remove the descending string to get the property name
                var indexOfFirstSpace = trimmedorderByStr.IndexOf(" ");
                string propertyName = indexOfFirstSpace == -1
                    ? trimmedorderByStr
                    : trimmedorderByStr.Remove(indexOfFirstSpace);

                // Determines whether the attribute name exists in the mappingDictionary
                if (!mappingDictionary.ContainsKey(propertyName)) throw new ArgumentException($"Key mapping for {propertyName} is missing.");
                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue is null) throw new ArgumentException("propertyMappingValue");

                // Linq.Dynamic OrderBy
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    orderByString = orderByString + (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ",")
                        + destinationProperty
                        + (isOrderByDescending ? " descending" : " ascending");
                }
            }
            return source.OrderBy(orderByString);
        }

        // orderBy string pattern: "modelPropertyName" or "modelPropertyName desc" or "modelPropertyName, modelPropertyName desc"
        /// <summary>
        /// IQueryable Extensions ApplySort with Model Property Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy)
        {
            if (source is null) throw new ArgumentNullException("source");
            if (string.IsNullOrEmpty(orderBy)) return source;

            var orderByString = string.Empty;
            var orderByAfterSplit = orderBy.Split(',');
            foreach (var orderByStr in orderByAfterSplit)
            {
                // Remove spaces
                var trimmedorderByStr = orderByStr.Trim();
                if (string.IsNullOrWhiteSpace(trimmedorderByStr)) break;
                // Determine sort direction
                var isOrderByDescending = trimmedorderByStr.EndsWith("desc");
                // Remove the descending string to get the property name
                var indexOfFirstSpace = trimmedorderByStr.IndexOf(" ");
                // get property Name
                string propertyName = indexOfFirstSpace == -1
                    ? trimmedorderByStr
                    : trimmedorderByStr.Remove(indexOfFirstSpace);

                // Determines whether the attribute name exists in the mappingDictionary
                if (IsPropertiesValidated<T>(propertyName))
                    orderByString = orderByString
                        + (!string.IsNullOrWhiteSpace(propertyName) ? string.Empty : ",")
                        + propertyName
                        + (isOrderByDescending ? " descending" : " ascending");
                else throw new ArgumentException($"Order by parameter: {propertyName} is not validated model Property");
            }
            return source.OrderBy(orderByString);
        }

        private static bool IsPropertiesValidated<T>(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) return true;
            propertyName = propertyName.Trim();
            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo is null) return false;
            return true;
        }
    }
}