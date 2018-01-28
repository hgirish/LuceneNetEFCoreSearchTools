using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LuceneNetEFCoreSearchTools.Web.Models;

namespace LuceneNetEFCoreSearchTools.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly LuceneIndexerOptions _luceneIndexerOptions;
        private readonly SearchableContextProvider<AppDbContext> _searchProvider;

        public HomeController(AppDbContext appDbContext, LuceneIndexerOptions luceneIndexerOptions,
            SearchableContextProvider<AppDbContext> searchProvider)
        {
            _appDbContext = appDbContext;
            _luceneIndexerOptions = luceneIndexerOptions;
            _searchProvider = searchProvider;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CreateIndex()
        {
            //SearchableContextProvider<AppDbContext> searchProvider = 
            //    new SearchableContextProvider<AppDbContext>();
           // LuceneIndexerOptions options = new LuceneIndexerOptions { UseRamDirectory = true };

            _searchProvider.Initialize(_luceneIndexerOptions, _appDbContext);

            _searchProvider.CreateIndex();
            var indexCounts = _searchProvider.IndexCount;
            ViewData["Message"] = $"index counts: {indexCounts}";
            return View("About");


        }

        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost][ValidateAntiForgeryToken]
        public IActionResult AddUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return View();

            }
            _searchProvider.Initialize(_luceneIndexerOptions, _appDbContext);
            _searchProvider.Context.Users.Add(user);
            _searchProvider.SaveChanges();
            return View();
        }
    }
}
