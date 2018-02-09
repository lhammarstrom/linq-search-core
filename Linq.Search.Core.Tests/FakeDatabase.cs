using System.Collections.Generic;
using System.Linq;
using System.Linq.Search.Core.Attributes;

namespace Linq.Search.Core.Tests
{
    public class FakeDatabase
    {
        public FakeDatabase()
        {
            SeedFacilities();
            SeedPeople();
        }

        private void SeedFacilities()
        {
            Facilities = new List<Facility>
            {
                new Facility("Corner Shop", "Spruce Street 1"),
                new Facility("Batcave", "Robinson Park Reservoir")
            };
        }

        private void SeedPeople()
        {
            Persons = new List<Person>
            {
                new Person("Mort Goldman", "Potatoes", Facilities.ToArray()[0]),
                new Person("Mary Poppins", "Caramels", Facilities.ToArray()[1]),
                new Person("Bruce Wayne", "Criminals", Facilities.ToArray()[1])
            };
        }

        public IEnumerable<Person> Persons { get; set; }

        public IEnumerable<Facility> Facilities { get; set; }
    }

    public class Person
    {
        public Person(string name, string favoriteFood, Facility facility, string description = "")
        {
            Name = name;
            FavoriteFood = favoriteFood;
            Facility = facility;
            Description = description;
        }

        [Searchable]
        public string Name { get; set; }

        [Searchable]
        public string FavoriteFood { get; set; }

        public string Description { get; set; }

        [Searchable]
        public Facility Facility { get; set; }
    }

    public class Facility
    {
        public Facility(string name, string address)
        {
            Name = name;
            Address = address;
        }

        [Searchable]
        public string Name { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Person> Persons { get; set; }
    }
}
