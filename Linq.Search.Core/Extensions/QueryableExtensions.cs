using System.Linq;
using System.Linq.Dynamic.Core;

namespace Linq.Search.Core.Extensions
{
    public static class QueryableExtensions
    {
        // extension which allows one to dynamically query data
        // depending on whether the property that one wants to query by
        // has the attribute [Searchable] set.
        public static IQueryable<T> SearchFor<T>(this IQueryable<T> source, string search)
        {
            // if searchstring is empty, return source, very rarely we want to search
            // for "nothing", so we left it out
            if (search.IsEmpty()) return source;

            // get all search-entries that the user wants to search for.
            // one can enter multplie queries if they are divided by |
            var entries = search.Split('|');

            // trim all unwanted spaces from sub-searchstring that we want to search for
            for (var i = 0; i < entries.Length; i++) entries[i] = entries[i].Trim();

            // get the names of all the properties that are
            // searchable (With the [Searchable] attribute)
            // we use Dynamic.Linq to build a query which uses .Contains
            // to see if our property contains the sub-searchstring
            var parentproperties = typeof(T).SearchablePropertiesOfType<string>()
                .SelectMany(prop => entries.Select(e => prop.Name + ".Contains(\"" + e + "\")"));

            // get all the searchable child properties, e.g. those related objects
            // that are marked with the [Searchable] attribute
            // as above this uses .Contains to see if our property contains
            // our sub-searchstring
            var childproperties = typeof(T)
                .SearchableProperties()
                .SelectMany(prop =>
                    prop.PropertyType.SearchablePropertiesOfType<string>()
                        .SelectMany(child => entries
                            .Select(entry => prop.Name + "." + child.Name + ".Contains(\"" + entry + "\")")));

            // join the properties together to a list, we use List
            // so that multiple enumerations won't occur
            var properties = parentproperties
                .Union(childproperties)
                .ToList();

            // if there arent any searchable properties just return source,
            // as we can't search for anything in nothing
            if (!properties.Any()) return source;

            // join the list of strings to a string in the form: 

            // StringProperty1.Contains("search") || 
            // StringProperty2.Contains("search") || 
            // RelatedObject1.StringProperty3.Contains("search") || 
            // RelatedObject1.StringProperty4.Contains("search")
            var query = properties.JoinToString("||");

            // return the source as a new query
            return source.Where(query);
        }
    }
}
