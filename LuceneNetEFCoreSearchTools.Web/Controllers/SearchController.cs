using LuceneNetEFCoreSearchTools.Web.Models;
using LuceneNetEFCoreSearchTools.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuceneNetEFCoreSearchTools.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly LuceneIndexerOptions _luceneIndexerOptions;
        private readonly SearchableContextProvider<AppDbContext> _searchProvider;

        public SearchController(AppDbContext appDbContext, LuceneIndexerOptions luceneIndexerOptions,
            SearchableContextProvider<AppDbContext> searchProvider)
        {
            _appDbContext = appDbContext;
            _luceneIndexerOptions = luceneIndexerOptions;
            _searchProvider = searchProvider;
        }
        public IActionResult Index()
        {
            ViewBag.Message = "Search something";

            return View();
        }
        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult Index(string q)
        {

            if (string.IsNullOrEmpty(q))
            {
                ModelState.AddModelError("q", "search  string required");
                return View();
            }
            q = q + "*";
            _searchProvider.Initialize(_luceneIndexerOptions, _appDbContext);
            SearchOptions searchOptions = new SearchOptions(q, "FirstName,Surname,Email,JobTitle,Name,Country");

            var userResults = _searchProvider.ScoredSearch<User>(searchOptions);
            var cityResults = _searchProvider.ScoredSearch<City>(searchOptions);
            SearchResultViewModel viewModel = new SearchResultViewModel
            {
                UserSearchCollection = userResults,
                CitySearchCollection = cityResults
            };
            ViewBag.Message = $"Search completed: {q}";
            return View(viewModel);
        }
    }
}
