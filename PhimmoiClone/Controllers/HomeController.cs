using Microsoft.AspNetCore.Mvc;
using PhimmoiClone.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Areas.Cinema.Repository.MovieRepo;
using PhimmoiClone.Data;

namespace PhimmoiClone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieRepo _repo;
        private readonly MyDbContext _ctx;

        public HomeController(ILogger<HomeController> logger, IMovieRepo repo, MyDbContext ctx)
        {
            _logger = logger;
            _repo = repo;
            _ctx = ctx;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            var movies = await _ctx.Movies
                .Include(m => m.MovieImages)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int totalMovieCount = _ctx.Movies.Count();
            int totalPageCount = (int)Math.Ceiling((double)totalMovieCount / pageSize);

            ViewData["TotalPageCount"] = totalPageCount;
            ViewData["CurrentPage"] = page;
            
            return View(movies);
        }
        public async Task<IActionResult> Search(string searchTitle, int page = 1, int pageSize = 3)
        {
            var allMovies = _ctx.Movies
                .Include(m => m.MovieImages)
                .Where(m => m.Name.Contains(searchTitle));
            var movies = await allMovies.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            
            int totalMovieCount = _ctx.Movies.Count();
            int totalPageCount = (int)Math.Ceiling((double)totalMovieCount / pageSize);

            ViewData["TotalPageCount"] = totalPageCount;
            ViewData["CurrentPage"] = page;
            
            return View(movies);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var movie = await _repo.GetByIdAsync(id);
            if (movie == null)
                return NotFound();
            return View(movie);
        }
        
        public async Task<IActionResult> MovieWithEpisode(int movieId, int episodeId)
        {
            var movie = await _repo.GetByIdAsync(movieId);
            if (movie == null)
                return NotFound();

            var episodes = movie.Episodes?.OrderBy(m => m.EpNumber).ToList();
            ViewData["Episodes"] = episodes;
            ViewData["Movie"] = movie;
            var epsiode = episodes?.FirstOrDefault(ep => ep.Id == episodeId);
            if (epsiode == null)
                return NotFound();
            
            return View(epsiode);
        }
    }
}