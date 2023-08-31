using Microsoft.AspNetCore.Mvc;
using PhimmoiClone.Areas.Cinema.Repository.EpisodeRepo;
using PhimmoiClone.Areas.Cinema.Repository.MovieRepo;
using PhimmoiClone.Areas.Cinema.ViewModel;

namespace PhimmoiClone.Areas.Cinema.Controllers;
[Area("Cinema")]
[Route("movie-{movieId}-[action]")]
public class EpisodeController : Controller
{
    private readonly IMovieRepo _movieRepo;
    private readonly IEpisodeRepo _repo;
    private readonly ILogger<EpisodeController> _logger;

    public EpisodeController(
        IMovieRepo movieRepo,
        IEpisodeRepo repo,
        ILogger<EpisodeController> logger)
    {
        _movieRepo = movieRepo;
        _repo = repo;
        _logger = logger;
    }
    [TempData] 
    public string StatusMessage { get; set; }
    public async Task<IActionResult> Detail(int movieId, int id)
    {
        var movie = await _movieRepo.GetByIdAsync(movieId);
        var episode = await _repo.GetByIdAsync(id);
        ViewData["Movie"] = movie;
        return View(episode);
    }

    public async Task<IActionResult> Create(int movieId)
    {
        var movie = await _movieRepo.GetByIdAsync(movieId);
        if (movie == null)
            return NotFound();
        ViewData["Movie"] = movie;
        return View(new EpisodeViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(int movieId, EpisodeViewModel vm)
    {
        var movie = await _movieRepo.GetByIdAsync(movieId);
        if (movie == null)
            return NotFound();
        
        await _repo.CreateAsync(vm);
        StatusMessage = await _repo.SaveAsync() ? "tạo tập phim thành công" : "tạo tập phim thất bại";
        
        return RedirectToAction("Detail", "Movie", new { Id = movieId });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int movieId, int id)
    {
        await _repo.DeleteAsync(id);
        StatusMessage = await _repo.SaveAsync() ? "xóa tập phim thành công" : "xóa tập phim thất bại";
        return RedirectToAction("Detail", "Movie", new { Id = movieId });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int movieId,int id)
    {
        var movie = await _movieRepo.GetByIdAsync(movieId);
        var episode = await _repo.GetByIdAsync(id);
        
        if (episode == null)
            return NotFound();
        
        var vm = new EpisodeUpdateViewModel()
        {
            Id = episode.Id,
            EpNumber = episode.EpNumber,
            EpString = episode.EpString,
            LinkEmbed = episode.LinkEmbed
        };
        ViewData["Movie"] = movie;
        return View(vm);

    }

    [HttpPost]
    public async Task<IActionResult> Edit(int movieId, EpisodeUpdateViewModel vm)
    {
        await _repo.UpdateAsync(vm);
        StatusMessage = await _repo.SaveAsync() ? "cập nhật tập phim thành công" : "cập nhật tập phim thất bại";
        return RedirectToAction("Detail", "Movie", new { id = movieId });
    }
}