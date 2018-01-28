namespace LuceneNetEFCoreSearchTools.Interfaces
{
    public interface IScoredSearchResult<T>
    {
        float Score { get; set; }
        T Entity { get; set; }
    }
}
