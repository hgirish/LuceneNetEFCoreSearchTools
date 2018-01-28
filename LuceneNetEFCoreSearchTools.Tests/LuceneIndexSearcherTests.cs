using Lucene.Net.Store;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using System.Linq;
using Lucene.Net.Documents;
using Xunit;
using LuceneNetEFCoreSearchTools.Tests.Helpers;
using LuceneNetEFCoreSearchTools.Tests.Models;

namespace LuceneNetEFCoreSearchTools.Tests
{
    public class LuceneIndexSearcherTests : IClassFixture<TestDataGenerator>
    {
        // class setup
        static Directory directory = new RAMDirectory();
        static Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
        static TestDataGenerator tdg = new TestDataGenerator();

        // create an index
        static LuceneIndexer indexer = new LuceneIndexer(directory, analyzer);
        static LuceneIndexSearcher searcher = new LuceneIndexSearcher(directory, analyzer);

        public LuceneIndexSearcherTests(TestDataGenerator tdg)
        {
            indexer.CreateIndex(tdg.AllData);
        }
        //public static void Init(TestContext context)
        //{
        //    indexer.CreateIndex(tdg.AllData);
        //}

        [Fact]
        public void AnIndexCanBeSearched()
        {
            SearchOptions options = new SearchOptions("John", "FirstName");

            var results = searcher.ScoredSearch(options);

            Assert.Equal(5, results.TotalHits);
        }

        [Fact]
        public void SearchesCanBeDoneAcrossMultipleTypes()
        {
            SearchOptions options = new SearchOptions("John China", "FirstName, Country");
            var results = searcher.ScoredSearch(options);

            var firstType = results.Results.First().Document.GetField("Type");
            var lastType = results.Results[results.TotalHits - 1].Document.GetField("Type");

            Assert.NotEqual(firstType, lastType);
        }

        [Fact]
        public void TopNNumberOfResultsCanBeReturned()
        {
            SearchOptions options = new SearchOptions("China", "Country", 1000, null, typeof(City));

            var allResults = searcher.ScoredSearch(options);

            options.Take = 10;

            var subSet = searcher.ScoredSearch(options);

            for(var index = 0; index < 10; index++)
            {
                Assert.Equal(allResults.Results[index].Document.Get("IndexId"), subSet.Results[index].Document.Get("IndexId"));
            }

            Assert.Equal(10, subSet.Results.Count());
            Assert.Equal(allResults.TotalHits, subSet.TotalHits);  
        }

        [Fact]
        public void ResultsetCanBeSkippedAndTaken()
        {
            SearchOptions options = new SearchOptions("China", "Country", 1000, null, typeof(City));

            var allResults = searcher.ScoredSearch(options);

            options.Take = 10;
            options.Skip = 10;

            var subSet = searcher.ScoredSearch(options);

            for (var index = 0; index < 10; index++)
            {
                Assert.Equal(allResults.Results[index + 10].Document.Get("IndexId"), subSet.Results[index].Document.Get("IndexId"));
            }

            Assert.Equal(10, subSet.Results.Count());
            Assert.Equal(allResults.TotalHits, subSet.TotalHits);
        }

        [Fact]
        public void ResultsetCanBeOrdered()
        {
            SearchOptions options = new SearchOptions("John", "FirstName", 1000, null, typeof(User));

            var unordered = searcher.ScoredSearch(options);

            options.OrderBy.Add("Surname");

            var ordered = searcher.ScoredSearch(options);

            Assert.Equal(ordered.TotalHits, unordered.TotalHits);
            Assert.NotEqual(ordered.Results.First().Document.Get("Id"), unordered.Results.First().Document.Get("Id"));
        }

        [Fact]
        public void ASingleDocumentIsReturnedFromScoredSearchSingle()
        {
            SearchOptions options = new SearchOptions("jfisherj@alexa.com", "Email");

            var result = searcher.ScoredSearchSingle(options);

            Assert.NotNull(result);
            // Assert.InstanceOfType(result, typeof(Document));
            Assert.IsType<Document>(result);
            Assert.IsAssignableFrom<Document>(result);
            Assert.Equal("jfisherj@alexa.com", result.Get("Email"));
        }

        [Fact]
        public void MultipleResultsIsNotAProblemFromScoredSearchSingle()
        {
            SearchOptions options = new SearchOptions("John", "FirstName");

            var result = searcher.ScoredSearchSingle(options);

            Assert.NotNull(result);
            Assert.IsType<Document>(result);
            Assert.Equal("John", result.Get("FirstName"));
        }


    }
}
