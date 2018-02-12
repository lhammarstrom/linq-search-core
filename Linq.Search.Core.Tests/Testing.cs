using System.Linq;
using System.Linq.Search.Core;
using System.Linq.Search.Core.Extensions;
using Xunit;

namespace Linq.Search.Core.Tests
{
    public class Testing
    {
        // reference both Persons and Facilities as their own IQueryable
        // so that we can run tests on them to see if our search-function
        // works

        private readonly IQueryable<Person> _persons;

        private readonly IQueryable<Facility> _facilities;

        // initiate with seed from database
        public Testing()
        {
            // seeding and creation
            var db = new FakeDatabase();

            // get queryables
            _persons = db.Persons.AsQueryable();
            _facilities = db.Facilities.AsQueryable();
        }

        // there is only one person called Bruce Wayne in our database
        // therefore the test should reflect that fact

        [Fact]
        public void AmountOfBruceTest()
        {
            Assert.True(AmountOfPersonResultsFor("Bruce Wayne") == 1);
        }

        [Fact]
        public void AmountOfBruceTestList()
        {
            var people = _persons.ToList().SearchFor("Bruce Wayne").Count();
            Assert.True(people == 1);
        }

        // there are currently two people living in the batcave,
        // our test-results should reflect that

        [Fact]
        public void AmountOfPeopleInBatcaves()
        {
            Assert.True(AmountOfPersonResultsFor("Batcave") == 2);
        }

        [Fact]
        public void AmountOfPeopleInBatcaveList()
        {
            var people = _persons.ToList().SearchFor("Batcave").Count();
            Assert.True(people == 2);
        }

        // there is currently only one batcave in our database
        // our test-results should reflect that if we search for batcave

        [Fact]
        public void AmountOfBatcaves()
        {
            Assert.True(AmountOfFacilityResultsFor("Batcave") == 1);
        }

        [Fact]
        public void AmountOfBatcavesList()
        {
            var facilities = _facilities.ToList().SearchFor("Batcave").Count();
            Assert.True(facilities == 1);
        }

        [Fact]
        public void PeopleInBatcave()
        {
            var people = _persons.SearchFor("Batcave");
            Assert.True(people.Count(p => p.Name == "Bruce Wayne") == 1 &&
                        people.Count(p => p.Name == "Mary Poppins") == 1);
        }

        // test subqeury
        [Fact]
        public void DisallowSubquery()
        {
            var people = _persons.SearchFor("Bruce|Batcave", false).ToArray();
            Assert.True(!people.Any());
        }

        [Fact]
        public void SearchablePersonProperties()
        {
            var props = typeof(Person).SearchableProperties().Count();
            Assert.True(props == 3);
        }

        [Fact]
        public void SearchableFacilityProperties()
        {
            var props = typeof(Facility).SearchableProperties().Count();
            Assert.True(props == 1);
        }

        // private properties for testing
        private int AmountOfPersonResultsFor(string search) => _persons.SearchFor(search).Count();
        private int AmountOfFacilityResultsFor(string search) => _facilities.SearchFor(search).Count();
    }
}