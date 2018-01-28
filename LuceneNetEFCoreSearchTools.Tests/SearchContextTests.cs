using LuceneNetEFCoreSearchTools.Interfaces;
using LuceneNetEFCoreSearchTools.Tests.Helpers;
using LuceneNetEFCoreSearchTools.Tests.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LuceneNetEFCoreSearchTools.Tests
{
    [Trait("Category", "SearchContext")]
   public class SearchContextTests
    {
        private TestDbContext context;
        TestDataGenerator tdg = new TestDataGenerator();
        private readonly ITestOutputHelper _output;
        string tempName;
        public SearchContextTests(ITestOutputHelper output)
        {
            // tempName = System.IO.Path.GetTempFileName();
            //var dboptions = new DbContextOptionsBuilder<TestDbContext>()
            //   .UseInMemoryDatabase(databaseName: tempName)
            //   .Options;
            //context = new TestDbContext(dboptions);
            //var data = tdg.AllTestUsers;
            _output = output;
           // _output.WriteLine($"DatabaseName: {tempName}");
        }
        private void InitializeContext()
        {
            tempName = System.IO.Path.GetTempFileName();
            var dboptions = new DbContextOptionsBuilder<TestDbContext>()
               .UseInMemoryDatabase(databaseName: tempName)
               .Options;
            context = new TestDbContext(dboptions);
        }
        [Fact]
        public void AContextProviderCanIndexADatabase()
        {
            InitializeContext();
            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };
            searchProvider.Initialize(options, context);

            searchProvider.CreateIndex();

            Assert.Equal(2000, searchProvider.IndexCount);

            // cleanup
            searchProvider.DeleteIndex();
        }
        [Fact]
        public void AContextCanBeSearchedUsingAContextProvider()
        {
            InitializeContext();


            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };

            searchProvider.Initialize(options, context);

            searchProvider.CreateIndex();

            SearchOptions searchOptions = new SearchOptions("John", "FirstName");

            // test
            var results = searchProvider.ScoredSearch<User>(searchOptions);

            Assert.Equal(5, results.TotalHits);

            // cleanup
            searchProvider.DeleteIndex();
        }

        [Fact]
        public void SkipAndTakeWorkWhenSearchingUsingAContextProvider()
        {
            InitializeContext();
            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };

            searchProvider.Initialize(options, context);

            searchProvider.CreateIndex();

            SearchOptions searchOptions = new SearchOptions("John", "FirstName");

            // test
            var initialResults = searchProvider.ScoredSearch<User>(searchOptions);
            int lastId = initialResults.Results[4].Entity.Id;

            Assert.Equal(5, initialResults.TotalHits);
            Assert.Equal(5, initialResults.Results.Count());

            searchOptions.Skip = 4;
            searchOptions.Take = 1;
            var subResults = searchProvider.ScoredSearch<User>(searchOptions);

            Assert.Equal(5, subResults.TotalHits);
            Assert.Equal(1, subResults.Results.Count());
            Assert.Equal(lastId, subResults.Results.First().Entity.Id);

            // cleanup
            searchProvider.DeleteIndex();
        }

        [Fact]
        public void AContextCanBeSearchedUsingAWildCard()
        {
            InitializeContext();
            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };

            searchProvider.Initialize(options, context);

            searchProvider.CreateIndex();

            SearchOptions searchOptions = new SearchOptions("Joh*", "FirstName");

            // test
            var results = searchProvider.ScoredSearch<User>(searchOptions);
            PrintResult(results);

            Assert.Equal(10, results.TotalHits);

            // cleanup
            searchProvider.DeleteIndex();
        }

       

        [Fact]
        public void ASearchWillReturnTheSameResultsAsAScoredSearch()
        {
            InitializeContext();
            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };

            searchProvider.Initialize(options, context);

            searchProvider.CreateIndex();

            SearchOptions searchOptions = new SearchOptions("Joh*", "FirstName");

            // test
            var results = searchProvider.Search<User>(searchOptions);

            Assert.Equal(10, results.TotalHits);

            // cleanup
            searchProvider.DeleteIndex();
        }

        [Fact]
        public void AScoredSearchWillOrderByRelevence()
        {
            InitializeContext();
            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };

            searchProvider.Initialize(options, context);

            searchProvider.CreateIndex();

            SearchOptions searchOptions = new SearchOptions("Jeremy Burns", "FirstName,Surname");

            var results = searchProvider.ScoredSearch<User>(searchOptions);

            var first = results.Results.First().Entity;
            var highest = results.Results.First().Score;
            var lowest = results.Results.Last().Score;

            Assert.True(highest > lowest);
            Assert.Equal("Jeremy", first.FirstName);
            Assert.Equal("Burns", first.Surname);

            searchProvider.DeleteIndex();
        }

        [Fact]
        public void ASearchWillStillOrderByRelevence()
        {
            InitializeContext();
            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };

            searchProvider.Initialize(options, context);

            searchProvider.CreateIndex();

            SearchOptions searchOptions = new SearchOptions("Jeremy Burns", "FirstName,Surname");

            var results = searchProvider.Search<User>(searchOptions);

            var first = results.Results.First();

            Assert.Equal("Jeremy", first.FirstName);
            Assert.Equal("Burns", first.Surname);

            searchProvider.DeleteIndex();
        }

        [Fact]
        public void ASearchCanIncludeAnOrderBy()
        {
            InitializeContext();
            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };

            searchProvider.Initialize(options, context);
            searchProvider.CreateIndex();

            SearchOptions search = new SearchOptions("Jeremy", "FirstName", 5, null, null, "Surname");

            var results = searchProvider.ScoredSearch<User>(search);
        }

        [Fact]
        public void ASearchCanReturnMultipleEntityTypes()
        {
            InitializeContext();
            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };
            searchProvider.Initialize(options, context);
            searchProvider.CreateIndex();

            SearchOptions query = new SearchOptions("John China", "FirstName,Country");

            var resultSet = searchProvider.ScoredSearch(query);

            Assert.NotEqual(resultSet.Results.First().Entity.Type, resultSet.Results.Last().Entity.Type);
        }

        [Fact]
        public void ASearchCanBeOrderedAcrossTypes()
        {
            InitializeContext();
            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };
            searchProvider.Initialize(options, context);
            searchProvider.CreateIndex();

            SearchOptions query = new SearchOptions("Moore China", "Surname,Country");
            query.OrderBy.Add("Name");

            var resultSet = searchProvider.ScoredSearch(query);

            Assert.NotEqual(resultSet.Results.First().Entity.Type, resultSet.Results.Last().Entity.Type);
        }

        [Fact]
        public void ASearchCanOrderByMultipleFields()
        {
            InitializeContext();
            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };

            User jc = new User()
            {
                FirstName = "John",
                Surname = "Chapman",
                JobTitle = "Test Engineer",
                IndexId = Guid.NewGuid(),
                Email = "john.chapman@test.com"
            };

            searchProvider.Initialize(options, context);

            context.Users.Add(jc);
            context.SaveChanges();

            searchProvider.CreateIndex();

            SearchOptions search = new SearchOptions("John", "FirstName", 1000, null, null, "Surname,JobTitle");

            var results = searchProvider.ScoredSearch<User>(search);

            var topResult = results.Results[0];
            var secondResult = results.Results[1];

            Assert.Equal("Sales Associate", topResult.Entity.JobTitle);
            Assert.Equal("Test Engineer", secondResult.Entity.JobTitle);

            searchProvider.DeleteIndex();
        }

        [Fact]
        public void SaveChangesUpdatesEntitiesAddedToTheIndex()
        {
            InitializeContext();
            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };
            searchProvider.Initialize(options, context);
            searchProvider.CreateIndex();

            var newUser = new User()
            {
                FirstName = "Duke",
                Surname = "Nukem",
                Email = "duke.nukem@test.com",
                IndexId = Guid.NewGuid(),
                JobTitle = "Shooty Man"
            };

            var search = new SearchOptions("Nukem", "Surname");

            var initialResults = searchProvider.Search<User>(search);

            searchProvider.Context.Users.Add(newUser);
            searchProvider.SaveChanges(true);

            var newResults = searchProvider.Search<User>(search);

            Assert.Equal(0, initialResults.TotalHits);
            Assert.Equal(1, newResults.TotalHits);

            Assert.Equal(newUser.Id, newResults.Results[0].Id);
        }

        [Fact]
        public void SaveChangesDeletesRemovedEntitiesFromTheIndex()
        {
            InitializeContext();
            SearchableContextProvider<TestDbContext> searchProvider = new SearchableContextProvider<TestDbContext>();
            LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };
            searchProvider.Initialize(options, context);
            searchProvider.CreateIndex();

            var search = new SearchOptions("John", "FirstName");

            int initialIndexCount = searchProvider.IndexCount;

            var initialResults = searchProvider.Search<User>(search);
            int resultsCount = initialResults.TotalHits;

            // delete entities
            //
            // NOTE: Because the search result entities are attached to the context
            // we can easily do linq operations on them
            //
            searchProvider.Context.Users.RemoveRange(initialResults.Results);
            searchProvider.SaveChanges(true);

            var updatedSearch = searchProvider.Search<User>(search);
            int finalIndexCount = searchProvider.IndexCount;

            Assert.Equal(0, updatedSearch.TotalHits);
            Assert.Equal(0, updatedSearch.Results.Count);
            Assert.Equal((initialIndexCount - resultsCount), finalIndexCount);
        }


        [Fact]
        public void NonValidEntitiesAreIgnored()
        {
            InitializeContext();
            SearchableContextProvider<MockNonIndexableContext> searchProvider = new SearchableContextProvider<MockNonIndexableContext>();

            LuceneIndexerOptions indexerOptions = new LuceneIndexerOptions { UseRamDirectory = true };

            searchProvider.Initialize(indexerOptions);

            searchProvider.CreateIndex();

            Assert.Equal(0, searchProvider.IndexCount);
        }
        private void PrintResult(IScoredSearchResultCollection<User> results)
        {
            _output.WriteLine($"Total Hist: {results.TotalHits}\tTime Taken: {results.TimeTaken}");
            foreach (IScoredSearchResult<User> item in results.Results)
            {
                _output.WriteLine($"Score: {item.Score}\tName:{item.Entity.FirstName}\tSurname: {item.Entity.Surname}\tEmail: {item.Entity.Email}");

            }
        }
    }
}
