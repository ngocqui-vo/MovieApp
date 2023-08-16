using Microsoft.AspNetCore.Mvc;
using PhimmoiClone.Areas.Cinema.Repository.GenreRepo;
using PhimmoiClone.Areas.Cinema.ViewModel;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Cinema.Controllers;
[Area("Cinema")]
[Route("Genre")]
public class GenreController : Controller
{
    private readonly MyDbContext _ctx;
    private readonly IGenreRepo _repo;
    private readonly ILogger<GenreController> _logger;

    public GenreController(
        MyDbContext ctx,
        IGenreRepo repo,
        ILogger<GenreController> logger)
    {
        _ctx = ctx;
        _repo = repo;
        _logger = logger;
    }

    [TempData] 
    public string StatusMessage { get; set; }
    public async Task<IActionResult> Index()
    {
        var genres = await _repo.GetAllAsync();
        return View(genres);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var genre = await _repo.GetByIdAsync(id);
        return View(genre);
    }

    public IActionResult Create()
    {
        return View(new GenreViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(GenreViewModel vm)
    {
        await _repo.CreateAsync(vm);
        StatusMessage = await _repo.SaveAsync() ? "thêm thành công" : "thêm thất bại";
        return RedirectToAction("Create");
    }

    public async Task<IActionResult> DeleteConfirm(int id)
    {
        var genre = await _repo.GetByIdAsync(id);
        return View(genre);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.DeleteAsync(id);
        StatusMessage = await _repo.SaveAsync() ? "xóa thành công" : "xóa thất bại";
        return RedirectToAction("index");
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var genre = await _repo.GetByIdAsync(id);
        return View(genre);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, GenreViewModel vm)
    {
        await _repo.UpdateAsync(id, vm);
        StatusMessage = await _repo.SaveAsync() ? "cập nhật thành công" : "cập nhật thất bại";
        return RedirectToAction("Detail", new {id});
    }
}