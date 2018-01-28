using System;
using System.Collections.Generic;

namespace LuceneNetEFCoreSearchTools.Interfaces
{
    public interface ILuceneIndexSearcher
    {
        ILuceneSearchResultCollection ScoredSearch(SearchOptions options);
        ILuceneSearchResultCollection ScoredSearch(
            string searchText,
            string fields,
            int maximumNumberOfHits,
            Dictionary<string, float> boosts,
            Type type,
            string sortBy,
            int? skip,
            int? take);
    }
}
