using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Search.Core.Extensions;

namespace System.Linq.Search.Core
{
    public static class Search
    {
        // abstraction that allows searching in IEnumerables by
        // casting it to an iqueryable first, and then running
        // the query normally
        public static IEnumerable<T> SearchFor<T>(this IEnumerable<T> source,
                                                  string search,
                                                  bool allowSubqueries = true,
                                                  bool searchInChildProperties = true,
                                                  bool allowWhitespace = true)
        {
            return source.AsQueryable()
                         .SearchFor(search, allowSubqueries, searchInChildProperties, allowWhitespace);
        }

        // extension which allows one to dynamically query data
        // depending on whether the property that one wants to query by
        // has the attribute [Searchable] set.
        public static IQueryable<T> SearchFor<T>(this IQueryable<T> source,
                                                 string search,
                                                 bool allowSubqueries = true,
                                                 bool searchInChildProperties = true,
                                                 bool allowWhitespace = true)
        {
            // if searchstring is empty, return source, very rarely we want to search
            // for "nothing", so we left it out
            if (search.IsBlank()) return source;

            // get all search-entries that the user wants to search for.
            // one can enter multiple queries if they are divided by || or &&
            // if the allowSubqueries bool is set to false, then query will be able
            // to contain | as part of searchQuery
            var splitter = search.Contains("&&") ? "&&" : "||";
            var entries = allowSubqueries ? search.Split(new[] {splitter}, StringSplitOptions.None) : new[] {search};

            // get the names of all the properties that are
            // searchable (With the [Searchable] attribute)
            // we use Dynamic.Linq to build a query which uses .Contains
            // to see if our property contains the sub-searchstring
            var parentproperties = typeof(T).SearchablePropertiesOfType<string>()
                                            .SelectMany(prop => entries.Select(e => prop.Name + ".Contains(\"" + (allowWhitespace ? e : e.Trim()) + "\")"));

            // start by setting the total properties to parentproperties,
            // as we will later union this list with the searchable childproperties
            // if the user has set the bool searchInChildProperties to true
            var properties = parentproperties.ToList();

            // only run if we want to search in childproperties
            if (searchInChildProperties)
            {
                // get all the searchable child properties, e.g. those related objects
                // that are marked with the [Searchable] attribute
                // as above this uses .Contains to see if our property contains
                // our sub-searchstring. One can also disallow searching in childproperties
                // by setting the searchInChildProperties to false
                var childproperties = typeof(T).SearchableProperties()
                                               .SelectMany(prop => prop.PropertyType.SearchablePropertiesOfType<string>()
                                                                       .SelectMany(child => entries.Select(entry => prop.Name + "." + child.Name + ".Contains(\"" + (allowWhitespace ? entry : entry.Trim()) + "\")")));

                // join the properties together to a list, we use List
                // so that multiple enumerations won't occur
                properties = properties.Union(childproperties)
                                       .ToList();
            }

            // if there arent any searchable properties just return source,
            // as we can't search for anything in nothing
            if (!properties.Any()) return source;

            // join the list of strings to a string in the form: 

            // StringProperty1.Contains("search") || 
            // StringProperty2.Contains("search") || 
            // RelatedObject1.StringProperty3.Contains("search") || 
            // RelatedObject1.StringProperty4.Contains("search")
            var query = properties.JoinToString(splitter);

            // return the source as a new query
            return source.Where(query);
        }
    }
}