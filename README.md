# Linq.Search.Core
[![Build status](https://ci.appveyor.com/api/projects/status/u8370f2si1qvreva?svg=true)](https://ci.appveyor.com/project/lhammarstrom/linq-search-core)
[![codecov](https://codecov.io/gh/lhammarstrom/linq.search.core/branch/master/graph/badge.svg)](https://codecov.io/gh/lhammarstrom/linq.search.core)
[![GitHub issues](https://img.shields.io/github/issues/lhammarstrom/linq.search.core.svg)](https://github.com/lhammarstrom/linq.search.core/issues)
[![GitHub stars](https://img.shields.io/github/stars/lhammarstrom/linq.search.core.svg)](https://github.com/lhammarstrom/linq.search.core/stargazers)
[![Gitter chat](https://badges.gitter.im/linq-search-core/gitter.png)](https://gitter.im/linq-search-core)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/lhammarstrom/linq.search.core/master/LICENSE)

A fast and easy way to implement scalable searching with Linq in .NET. It uses reflection to gather all properties that one wants to search in, and then runs a query using dynamic linq and .Contains("{search}") to return all results which match the searchstring.

### Improvements and general discussion 
If you have any thoughts, improvements, or just want to chat about Linq.Search.Core, go to our [Gitter chat](https://gitter.im/linq-search-core) and join in on the discussions.

### Installing
[![NuGet Badge](https://buildstats.info/nuget/Linq.Search.Core)](https://www.nuget.org/packages/Linq.Search.Core)

This package is available on nuget, run your available command and it should install nicely.

#### NPM console
```
npm install Linq.Search.Core
```

#### Package Manager
```
Install-Package Linq.Search.Core
```

#### .NET CLI
```
dotnet add package Linq.Search.Core
```

### Usage

Add the ```[Searchable]``` attribute to any property or related object that you want to be able to index searches by. For example, say that you have some sort of database consisting of ```Persons``` which in turn are connected to ```Facilities```, then the classes would look something like this:

```
public class Person
{
    [Searchable]
    public string Name { get; }
    
    public string Description { get; }
    
    [Searchable]
    public Facility Facility { get; }
}
```

```
public class Facility
{
    [Searchable]
    public string Name { get; }
    
    public virtual ICollection<Person> Persons { get; set; }
}
```

#### Example 1
Search for Bruce

```
public class Testing {
    
    public IQueryable<Person> SearchForBruce()
    {
        var persons = new List<Person>
        {
            new Person
            {
                Name = "Bruce Wayne",
                Description = "Batman",
                Facility = new Facility
                {
                    Name = "Batcave"
                }
            }
        };
        
        return persons
            .AsQueryable()
            .SearchFor("Bruce");
    }
}
```

The search-function is also built to support queries with multiple subqueries. For example if one would want to search for any elements matching "Bruce" or "Batcave". To do this, all search queries would need to be separated by "|".

#### Example 2
Search for Bruce OR Batcave.

```
return persons
    .AsQueryable()
    .SearchFor("Bruce|Batcave");
```

## Upcoming stuff
* Implement .SearchFor() for all types of IEnumerables!
* See if we can cut down dependencies.
* Implement Levehstein-distance to order search-results by significance.

## Built With

* [System.Linq.Dynamic.Core](https://github.com/StefH/System.Linq.Dynamic.Core) - Dynamic Linq used to run queries.

## Authors

* **Lennart Hammarström** - [lhammarstrom](https://github.com/lhammarstrom)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Stef Heyenrath - Dynamic Linq - [System.Linq.Dynamic.Core](https://github.com/StefH/System.Linq.Dynamic.Core)
* Yannick Lung - Icon - [IconFinder](https://www.iconfinder.com/yanlu)
