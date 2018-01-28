using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LuceneNetEFCoreSearchTools.Interfaces
{
    
    public interface ISearchableContextProvider<TEntity> where TEntity :  class, new()
    {
        TEntity Context { get; }
        int IndexCount { get; }

        void CreateIndex(bool optimize = false);
        void DeleteIndex();
        void Initialize(LuceneIndexerOptions indexerOptions, bool overrideIfExists);
        void Initialize(LuceneIndexerOptions indexerOptions, TEntity context, bool overrideIfExists);
        int SaveChanges(bool index = true);
        Task<int> SaveChangesAsync(bool index = true);
        IScoredSearchResultCollection<ILuceneIndexable> ScoredSearch(SearchOptions options);
        IScoredSearchResultCollection<T> ScoredSearch<T>(SearchOptions options);
        ISearchResultCollection<ILuceneIndexable> Search(SearchOptions options);
        ISearchResultCollection<T> Search<T>(SearchOptions options);
        //DbSet<CTX> Set(Type t);
    }
    public interface ISearchableContextProvider
    {
        
        int IndexCount { get; }

        void CreateIndex(bool optimize = false);
        void DeleteIndex();
        void Initialize(LuceneIndexerOptions indexerOptions, bool overrideIfExists);
      //  void Initialize(LuceneIndexerOptions indexerOptions, CTX context, bool overrideIfExists);
        int SaveChanges(bool index = true);
        Task<int> SaveChangesAsync(bool index = true);
        IScoredSearchResultCollection<ILuceneIndexable> ScoredSearch(SearchOptions options);
        IScoredSearchResultCollection<T> ScoredSearch<T>(SearchOptions options);
        ISearchResultCollection<ILuceneIndexable> Search(SearchOptions options);
        ISearchResultCollection<T> Search<T>(SearchOptions options);
        //DbSet<CTX> Set(Type t);
    }
}
