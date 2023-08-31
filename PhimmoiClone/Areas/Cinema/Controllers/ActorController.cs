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
        var actors = await _repo.GetAllAsync();
        return View(actors);
    }

    [HttpGet("get-actor-{id}")]
    public async Task<IActionResult> Detail(int? id)
    {
        if (id == null)
            return NotFound();
        var actor = await _repo.GetByIdAsync((int)id);
        
        var movies = actor?.MovieActors?.Select(ma => ma.Movie).ToList();
        ViewData["Movies"] = movies;
        return View(actor);
    }

    [HttpGet("create-actor")]
    public IActionResult Create()
    {
        return View(new ActorViewModel());
    }

    [HttpPost("create-actor")]
    public async Task<IActionResult> Create(ActorViewModel actorViewModel)
    {
        await _repo.CreateAsync(actorViewModel);
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
    public async Task<IActionResult> DeleteConfirm(int? id)
    {
        if (id == null)
            return NotFound();
        var actor = await _repo.GetByIdAsync((int)id);
        return View(actor);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        if (await _repo.SaveAsync())
        {
            StatusMessage = "Xóa thành công";
            return RedirectToAction("Index");
        }

        StatusMessage = "Xóa thất bại";
        return RedirectToAction("index");

    }
    [HttpGet("edit-actor-{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var actor = await _repo.GetByIdAsync(id);
        
        if (actor == null)
            return NotFound();

        var actorEditViewModel = new ActorEditViewModel
        {
            Id = actor.Id,
            Name = actor.Name,
            Description = actor.Description,
            DoB = actor.DoB,
            ExistingImage = actor.Image,
            Image = null
        };
        return View(actorEditViewModel);
    }

    [HttpPost("edit-actor-{id}")]
    public async Task<IActionResult> Edit(int id, ActorEditViewModel actorEditViewModel)
    {
        var actor = await _repo.GetByIdAsync(id);
        if (actor == null)
            return NotFound();
        await _repo.UpdateAsync(id, actorEditViewModel);
        if (await _repo.SaveAsync())
        {
            StatusMessage = "cập nhật thành công";
            return RedirectToAction("Detail", new { id });
        }

        StatusMessage = "cập nhật thất bại";
        return RedirectToAction("Detail", new { id });
    }

    
}