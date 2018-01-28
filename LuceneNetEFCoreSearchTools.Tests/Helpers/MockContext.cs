using LuceneNetEFCoreSearchTools.Tests.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LuceneNetEFCoreSearchTools.Tests.Helpers
{
    public class MockContext : DbContext
    {
       
        public virtual DbSet<User> Users {get;set;}
        
    }
}
