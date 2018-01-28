using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Store;
using LuceneNetEFCoreSearchTools.Tests.Helpers;
using LuceneNetEFCoreSearchTools.Tests.Models;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace LuceneNetEFCoreSearchTools.Tests
{
    [Trait("Category","LuceneIndexer")]
    public class LuceneIndexerTests : IDisposable
    {
        //TestDataGenerator tdg = new TestDataGenerator();
        //Directory directory = new RAMDirectory();
        //Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
        private readonly ITestOutputHelper output;
        private LuceneIndexer indexer;

        public LuceneIndexerTests(ITestOutputHelper output)
        {
                       this.output = output;
        }
        //private void Initialize()
        //{
        //    TestDataGenerator tdg = new TestDataGenerator();
        //    Directory directory = new RAMDirectory();
        //    Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
        //    indexer = new LuceneIndexer(directory, analyzer);
        //    indexer.CreateIndex(tdg.AllData, true);
        //}
        public void Dispose()
        {
            
        }
        [Fact]
        public void AnIndexCanBeCreated()
        {
            TestDataGenerator tdg = new TestDataGenerator();
            Directory directory = new RAMDirectory();
            
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            indexer = new LuceneIndexer(directory, analyzer);
            indexer.CreateIndex(tdg.AllData, true);
            Assert.Equal(2000, indexer.Count());
            analyzer.Dispose();
            directory.ClearLock("write.lock");
            directory.Dispose();
            
        }
        [Fact]
        public void AnIndexCanBeDeleted()
        {
            TestDataGenerator tdg = new TestDataGenerator();
            Directory directory = new RAMDirectory();
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            indexer = new LuceneIndexer(directory, analyzer);
            indexer.CreateIndex(tdg.AllData, true);
            Assert.Equal(2000, indexer.Count());

            indexer.DeleteAll(true);

            Assert.Equal(0, indexer.Count());
            directory.ClearLock("write.lock");
            analyzer.Dispose();
            directory.Dispose();
        }
        [Fact]
        public void AnItemCanBeAddedToTheIndex()
        {
            TestDataGenerator tdg = new TestDataGenerator();
            Directory directory = new RAMDirectory();
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            indexer = new LuceneIndexer(directory, analyzer);
            indexer.CreateIndex(tdg.AllData, true);
            Assert.Equal(2000, indexer.Count());

            indexer.Add(tdg.ANewUser());

            Assert.Equal(2001, indexer.Count());
            directory.ClearLock("write.lock");
        }
        [Fact]
        public void AnItemCanBeRemovedFromTheIndex()
        {
            TestDataGenerator tdg = new TestDataGenerator();
            Directory directory = new RAMDirectory();
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            indexer = new LuceneIndexer(directory, analyzer);
            indexer.CreateIndex(tdg.AllData, true);
            indexer.Delete(tdg.AllData.First());

            Assert.Equal(1999, indexer.Count());
            directory.ClearLock("write.lock");
        }

        [Fact]
        public void AnItemCanBeUpdatedInTheIndex()
        {
            TestDataGenerator tdg = new TestDataGenerator();
            Directory directory = new RAMDirectory();
            Analyzer analyzer = new StandardAnalyzer(Lucene.Net.Util.LuceneVersion.LUCENE_48);
            indexer = new LuceneIndexer(directory, analyzer);
            indexer.CreateIndex(tdg.AllData, true);

            // we need a searcher for this test
            LuceneIndexSearcher searcher = new LuceneIndexSearcher(directory, analyzer);

            // get the 1st item
            SearchOptions options = new SearchOptions("ghudson0@rambler.ru", "Email");

            var initialResults = searcher.ScoredSearch(options);
            foreach (var item in initialResults.Results)
            {
                
                output.WriteLine($"{item.Score}\t{item.Document.Get("Id")}\t{item.Document.Get("FirstName")}\t{item.Document.Get("Email")}");
            }
            //Assert.Equal(1, initialResults.TotalHits);

            Document rambler = initialResults.Results.First().Document;

            // convert to ILuceneIndexable
            User user = new User()
            {
                Id = int.Parse(rambler.Get("Id")),
                IndexId = new Guid(rambler.Get("IndexId")),
                FirstName = rambler.Get("FirstName"),
                Surname = rambler.Get("Surname"),
                Email = rambler.Get("Email"),
                JobTitle = rambler.Get("JobTitle")
            };

            // make an edit
            user.FirstName = "Duke";
            user.Surname = "Nukem";

            // add the update to the indexer
            indexer.Update(user);

            // search again
            var endResults = searcher.ScoredSearch(options);
            foreach (var item in endResults.Results)
            {
                output.WriteLine($"{item.Score}\t{item.Document.Get("Id")}\t{item.Document.Get("FirstName")}\t{item.Document.Get("Email")}");
            }

           // Assert.Equal(1, endResults.TotalHits);
            Assert.Equal(user.IndexId.ToString(), endResults.Results.First().Document.Get("IndexId"));
            Assert.Equal(user.Id.ToString(), endResults.Results.First().Document.Get("Id"));
            Assert.Equal(user.FirstName, endResults.Results.First().Document.Get("FirstName"));
            Assert.Equal(user.Surname, endResults.Results.First().Document.Get("Surname"));
            directory.ClearLock("write.lock");
        }

       
    }
}
