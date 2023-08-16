using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Areas.Cinema.Models;
using PhimmoiClone.Areas.Cinema.Repository.ActorRepo;
using PhimmoiClone.Areas.Cinema.ViewModel;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Cinema.Controllers;

[Area("Cinema")]
[Route("Actor/[action]")]
public class ActorController : Controller
{
    private readonly IActorRepo _repo;
    private readonly MyDbContext _ctx;
    private readonly ILogger<ActorController> _logger;

    public ActorController(IActorRepo repo, MyDbContext ctx, ILogger<ActorController> logger)
    {
        _repo = repo;
        _ctx = ctx;
        _logger = logger;
    }
    
    [TempData] 
    public string StatusMessage { get; set; }
    
    [HttpGet("get-all-actors")]
    public async Task<IActionResult> Index()
    {
        var actors = await _repo.GetAllActorAsync();
        return View(actors);
    }

    [HttpGet("get-actor-{id}")]
    public async Task<IActionResult> ActorDetail(int? id)
    {
        if (id == null)
            return NotFound();
        var actor = await _repo.GetActorByIdAsync((int)id);
        
        var movies = actor?.MovieActors?.Select(ma => ma.Movie).ToList();
        ViewData["Movies"] = movies;
        return View(actor);
    }

    [HttpGet("create-actor")]
    public IActionResult CreateActor()
    {
        return View(new ActorViewModel());
    }

    [HttpPost("create-actor")]
    public async Task<IActionResult> CreateActor(ActorViewModel actorViewModel)
    {
        await _repo.CreateActorAsync(actorViewModel);
        if (await _repo.SaveAsync())
        {
            StatusMessage = "thêm thành công";
        }
        else
        {
            StatusMessage = "thêm thất bại";
        }

        return RedirectToAction("Index");
    }
    
    [HttpGet("delete-actor-{id}")]
    public async Task<IActionResult> DeleteActorCornFirm(int? id)
    {
        if (id == null)
            return NotFound();
        var actor = await _repo.GetActorByIdAsync((int)id);
        return View(actor);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteActor(int id)
    {
        await _repo.DeleteActorAsync(id);
        if (await _repo.SaveAsync())
        {
            StatusMessage = "Xóa thành công";
            return RedirectToAction("Index");
        }

        StatusMessage = "Xóa thất bại";
        return RedirectToAction("index");

    }
    [HttpGet("update-actor-{id}")]
    public async Task<IActionResult> UpdateActor(int id)
    {
        var actor = await _repo.GetActorByIdAsync(id);
        return View(actor);
    }

    [HttpPost("update-actor-{id}")]
    public async Task<IActionResult> UpdateActor(int id, ActorViewModel actorViewModel)
    {
        var actor = await _repo.GetActorByIdAsync(id);
        if (actor == null)
            return NotFound();
        await _repo.UpdateActorAsync(id, actorViewModel);
        if (await _repo.SaveAsync())
        {
            StatusMessage = "cập nhật thành công";
            return RedirectToAction("Index", new { id = id });
        }

        StatusMessage = "cập nhật thất bại";
        return RedirectToAction("Index", new { id = id });
    }
}