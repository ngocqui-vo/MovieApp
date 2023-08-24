using Microsoft.AspNetCore.Mvc;
using PhimmoiClone.Areas.Cinema.Repository.EpisodeRepo;
using PhimmoiClone.Areas.Cinema.Repository.MovieRepo;

namespace PhimmoiClone.Areas.Cinema.Controllers;

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

    public async Task<IActionResult> Detail(int movieId, int id)
    {
        var movie = await _movieRepo.GetByIdAsync(movieId);
        var episode = await _repo.GetByIdAsync(id);
        ViewData["Movie"] = movie;
        return View(episode);
    }
}