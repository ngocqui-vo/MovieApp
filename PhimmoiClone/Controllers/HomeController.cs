using Microsoft.AspNetCore.Mvc;
using PhimmoiClone.Models;
using System.Diagnostics;
using PhimmoiClone.Areas.Cinema.Repository.MovieRepo;

namespace PhimmoiClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieRepo _repo;

        public HomeController(ILogger<HomeController> logger, IMovieRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public IActionResult Index(string searchTitle)
        {
            // var movies = 
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}