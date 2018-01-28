using LuceneNetEFCoreSearchTools.Interfaces;
using LuceneNetEFCoreSearchTools.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuceneNetEFCoreSearchTools.Web.ViewModels
{
    public class SearchResultViewModel
    {
       public IScoredSearchResultCollection<User> UserSearchCollection { get; set; }
       public IScoredSearchResultCollection<City> CitySearchCollection { get; set; }
    }
}
