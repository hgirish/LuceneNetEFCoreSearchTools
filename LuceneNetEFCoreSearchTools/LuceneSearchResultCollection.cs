using LuceneNetEFCoreSearchTools.Interfaces;
using System.Collections.Generic;

namespace LuceneNetEFCoreSearchTools
{
    public class LuceneSearchResultCollection : ILuceneSearchResultCollection
    {
        public IList<ILuceneSearchResult> Results { get; set; } = new List<ILuceneSearchResult>();

        public long TimeTaken { get; set; }

        public int TotalHits { get; set; }
    }
}