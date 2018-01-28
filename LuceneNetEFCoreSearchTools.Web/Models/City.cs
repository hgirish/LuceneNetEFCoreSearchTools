using System;

namespace LuceneNetEFCoreSearchTools.Web.Models
{
    public class City : LuceneIndexableEntityBase
    {

        [LuceneIndexable]
        public string Name { get; set; }

        public string Code { get; set; }

        [LuceneIndexable]
        public string Country { get; set; }

        public override Type Type { get { return typeof(City); } }
    }
}
