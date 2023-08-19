using Microsoft.AspNetCore.Mvc;
using PhimmoiClone.Areas.Cinema.Repository.ActorRepo;
using PhimmoiClone.Areas.Cinema.Repository.GenreRepo;
using PhimmoiClone.Areas.Cinema.Repository.MovieRepo;
using PhimmoiClone.Areas.Cinema.ViewModel;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Cinema.Controllers;

[Area("Cinema")]
[Route("Movie/[action]")]
public class MovieController : Controller
{
    private ILogger<MovieController> _logger;
    private IMovieRepo _repo;
    private IActorRepo _actorRepo;
    private IGenreRepo _genreRepo;
    private MyDbContext _ctx;

    public MovieController(
        ILogger<MovieController> logger,
        IMovieRepo repo,
        IActorRepo actorRepo,
        IGenreRepo genreRepo,
        MyDbContext ctx)
    {
        _logger = logger;
        _repo = repo;
        _actorRepo = actorRepo;
        _genreRepo = genreRepo;
        _ctx = ctx;
    }
    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> Index()
    {
        var movies = await _repo.GetAllAsync();
        return View(movies);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var movie = await _repo.GetByIdAsync(id);
        return View(movie);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var movie = await _repo.GetByIdAsync(id);
        return View(movie);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        StatusMessage = await _repo.SaveAsync() ? "xóa thành công" : "xóa thất bại";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View(new MovieViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(MovieViewModel vm)
    {
        await _repo.CreateAsync(vm);
        StatusMessage = await _repo.SaveAsync() ? "tạo thành công" : "tạo thất bại";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Actors"] = await _actorRepo.GetAllAsync();
        ViewData["Genres"] = await _genreRepo.GetAllAsync();

        var movie = await _repo.GetByIdAsync(id);
        return View(movie);
    }
}