using LuceneNetEFCoreSearchTools.Interfaces;
using System.Collections.Generic;

namespace LuceneNetEFCoreSearchTools
{
    public class SearchResultCollection<T> : ISearchResultCollection<T>
    {
        public IList<T> Results { get; set; }

        public long TimeTaken { get; set; }

        public int TotalHits { get; set; }

        public SearchResultCollection()
        {
            Results = new List<T>();
        }
    }

}
