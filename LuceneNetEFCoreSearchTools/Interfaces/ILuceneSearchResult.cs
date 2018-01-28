using Lucene.Net.Documents;

namespace LuceneNetEFCoreSearchTools.Interfaces
{
    public interface ILuceneSearchResult
    {
        float Score { get; set; }
        Document Document { get; set; }
    }
}
