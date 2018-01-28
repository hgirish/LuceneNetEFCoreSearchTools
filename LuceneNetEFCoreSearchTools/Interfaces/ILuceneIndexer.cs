using System.Collections.Generic;

namespace LuceneNetEFCoreSearchTools.Interfaces
{
    public interface ILuceneIndexer
    {
        void Add(ILuceneIndexable entity, bool optimize);
        void CreateIndex(IEnumerable<ILuceneIndexable> entities, bool recreate, bool optimize);
        void Delete(ILuceneIndexable entity, bool optimize);
        void DeleteAll(bool commit);
        void Update(ILuceneIndexable entity, bool optimize);
        void Update(LuceneIndexChange change, bool optimize);
        void Update(LuceneIndexChangeset changeset);

        int Count();
    }
}
