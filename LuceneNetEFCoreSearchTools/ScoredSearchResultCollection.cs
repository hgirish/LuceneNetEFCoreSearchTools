using LuceneNetEFCoreSearchTools.Interfaces;
using System.Collections.Generic;

namespace LuceneNetEFCoreSearchTools
{
    public class ScoredSearchResultCollection<T> : IScoredSearchResultCollection<T>
    {
        public IList<IScoredSearchResult<T>> Results { get; set; } = new List<IScoredSearchResult<T>>();

        public long TimeTaken { get; set; }

        public int TotalHits { get; set; }
    }

}
