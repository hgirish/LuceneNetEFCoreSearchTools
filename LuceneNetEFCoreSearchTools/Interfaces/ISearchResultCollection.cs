using System.Collections.Generic;

namespace LuceneNetEFCoreSearchTools.Interfaces
{
    public interface ISearchResultCollection<T>
    {
        int TotalHits { get; set; }
        long TimeTaken { get; set; }
        IList<T> Results { get; set; }
    }
}
