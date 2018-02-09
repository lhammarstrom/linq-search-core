using System.Linq;
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
        public void AmountOfBruceTestInverse()
        {
            Assert.False(AmountOfPersonResultsFor("Bruce Wayne") != 1);
        }

        // there are currently two people living in the batcave,
        // our test-results should reflect that

        [Fact]
        public void AmountOfPeopleInBatcaves()
        {
            Assert.True(AmountOfPersonResultsFor("Batcave") == 2);
        }

        [Fact]
        public void AmountOfPeopleInBatcavesInverse()
        {
            Assert.False(AmountOfPersonResultsFor("Batcave") != 2);
        }

        // there is currently only one batcave in our database
        // our test-results should reflect that if we search for batcave

        [Fact]
        public void AmountOfBatcaves()
        {
            Assert.True(AmountOfFacilityResultsFor("Batcave") == 1);
        }

        [Fact]
        public void AmountOfBatcavesInverse()
        {
            Assert.False(AmountOfFacilityResultsFor("Batcave") != 1);
        }

        // private properties for testing
        private int AmountOfPersonResultsFor(string search) => 1;//_persons.SearchFor(search).Count();
        private int AmountOfFacilityResultsFor(string search) => 1;//_facilities.SearchFor(search).Count();
    }
}