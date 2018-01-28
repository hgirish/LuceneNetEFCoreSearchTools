using Lucene.Net.Documents;
using LuceneNetEFCoreSearchTools.Interfaces;

namespace LuceneNetEFCoreSearchTools
{
    public class LuceneSearchResult : ILuceneSearchResult
    {
        public float Score { get; set; }
        public Document Document { get; set; }
    }

}
