using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Linq.Search.Core.Attributes;

namespace Linq.Search.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static IEnumerable<PropertyInfo> SearchableProperties(this IReflect obj) => obj
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => Attribute.IsDefined(p, typeof(Searchable)));

        public static IEnumerable<PropertyInfo> SearchablePropertiesOfType<T>(this IReflect obj) =>
            obj.SearchableProperties()
                .Where(p => p.PropertyType == typeof(T));
    }
}
