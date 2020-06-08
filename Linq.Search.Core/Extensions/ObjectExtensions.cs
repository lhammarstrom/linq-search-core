using System.Collections.Generic;
using System.Linq.Search.Core.Attributes;
using System.Reflection;

namespace System.Linq.Search.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static IEnumerable<PropertyInfo> SearchableProperties(this IReflect obj)
        {
            return obj.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                      .Where(p => Attribute.IsDefined(p, typeof(Searchable)));
        }

        public static IEnumerable<PropertyInfo> SearchablePropertiesOfType<T>(this IReflect obj)
        {
            return obj.SearchableProperties()
                      .Where(p => p.PropertyType == typeof(T));
        }
    }
}