using LuceneNetEFCoreSearchTools.Interfaces;

namespace LuceneNetEFCoreSearchTools
{
    public class ScoredSearchResult<T> : IScoredSearchResult<T>
    {
        public float Score { get; set; }
        public T Entity { get; set; }
    }


}
