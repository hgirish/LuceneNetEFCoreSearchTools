using LuceneNetEFCoreSearchTools.Testss.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LuceneNetEFCoreSearchTools.Tests.Helpers
{
    public class MockNonIndexableContext : DbContext
    {
        public virtual DbSet<NonIndexable> NonIndexables { get; set; }

       
    }
}
