using Microsoft.AspNetCore.Mvc;
using PhimmoiClone.Areas.Cinema.Repository.ActorRepo;
using PhimmoiClone.Areas.Cinema.Repository.EpisodeRepo;
using PhimmoiClone.Areas.Cinema.Repository.GenreRepo;
using PhimmoiClone.Areas.Cinema.Repository.MovieRepo;
using PhimmoiClone.Areas.Cinema.ViewModel;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Cinema.Controllers;

[Area("Cinema")]
[Route("Movie/[action]")]
public class MovieController : Controller
{
    private readonly ILogger<MovieController> _logger;
    private readonly IMovieRepo _repo;
    private readonly IActorRepo _actorRepo;
    private readonly IGenreRepo _genreRepo;
    private readonly IEpisodeRepo _episodeRepo;
    private readonly MyDbContext _ctx;

    public MovieController(
        ILogger<MovieController> logger,
        IMovieRepo repo,
        IActorRepo actorRepo,
        IGenreRepo genreRepo,
        IEpisodeRepo episodeRepo,
        MyDbContext ctx)    
    {
        _logger = logger;
        _repo = repo;
        _actorRepo = actorRepo;
        _genreRepo = genreRepo;
        _episodeRepo = episodeRepo;
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
        if (movie == null)
            return NotFound();

        var actors = movie.MovieActors?.Select(m => m.Actor).ToList();

        var genres = movie.MovieGenres?.Select(m => m.Genre).ToList();

        ViewData["Actors"] = actors;
        ViewData["Genres"] = genres;
        

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
    public IActionResult Create()
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

    [HttpPost]
    public async Task<IActionResult> Edit(int id, MovieViewModel vm)
    {
        await _repo.UpdateAsync(id, vm);
        StatusMessage = await _repo.SaveAsync() ? "cập nhật thành công" : "cập nhật thất bại";
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> AddActorsToMovie(int id)
    {
        var movie = await _repo.GetByIdAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        ViewData["Actors"] = await _actorRepo.GetAllAsync();
        
        ViewData["MovieActors"] = _repo.GetAllActorIds(movie);

        return View(movie);
    }

    [HttpPost]
    public async Task<IActionResult> AddActorsToMovie(int id, List<int> selectedActors)
    {
        var movie = await _repo.GetByIdAsync(id);
        if (movie != null)
        {
            // thêm actors mới
            var result = await _repo.AddToActorsAsync(movie, selectedActors);
            
            StatusMessage = result ? "gán diễn viên thành công" : "Error: gán diễn viên thất bại";
        }
        return RedirectToAction("Detail", new {id});
    }


    [HttpGet]
    public async Task<IActionResult> AddGenresToMovie(int id)
    {
        var movie = await _repo.GetByIdAsync(id);
        if (movie == null)
        {
            return NotFound();
        }
        ViewData["Genres"] = await _genreRepo.GetAllAsync();

        ViewData["MovieGenres"] = _repo.GetAllGenreIds(movie);

        return View(movie);
    }

    [HttpPost]
    public async Task<IActionResult> AddGenresToMovie(int id, List<int> selectedGenres)
    {
        var movie = await _repo.GetByIdAsync(id);
        if (movie != null)
        {


            // thêm actors mới
            var result = await _repo.AddToGenresAsync(movie, selectedGenres);

            StatusMessage = result ? "gán thể loại thành công" : "Error: gán thể loại thất bại";
        }
        return RedirectToAction("Detail", new { id });
    }

    
}