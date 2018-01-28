using System.Collections.Generic;

namespace LuceneNetEFCoreSearchTools.Interfaces
{
    public interface ILuceneSearchResultCollection
    {
        int TotalHits { get; set; }
        long TimeTaken { get; set; }
        IList<ILuceneSearchResult> Results { get; set; }
    }
}
