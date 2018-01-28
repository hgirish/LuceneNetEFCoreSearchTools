using Lucene.Net.Documents;
using System;

namespace LuceneNetEFCoreSearchTools.Interfaces
{
    public interface ILuceneIndexable
    {
        int Id { get; set; }
        Guid IndexId { get; set; }
        Type Type { get; }
        Document ToDocument();
    }
}
