using Microsoft.AspNetCore.Mvc;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Database.Controllers;

[Area("Database")]
[Route("Database/[action]")]
public class DatabaseController : Controller
{
    private readonly MyDbContext _ctx;

    public DatabaseController(MyDbContext ctx)
    {
        _ctx = ctx;
    }
    [TempData] 
    public string StatusMessage { get; set; }
    // GET
    public IActionResult Index()
    {
        
        return View(_ctx);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDatabase()
    {
        if (await _ctx.Database.CanConnectAsync())
        {
            StatusMessage = "Database đã tồn tại";
        }
        else
        {
            var result = await _ctx.Database.EnsureCreatedAsync();
            if (result)
            {
                StatusMessage = "Tạo thành công database";
                return RedirectToAction("Index");
            }

            StatusMessage = "Tạo database thất bại";
        }

        
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteDatabase()
    {
        if (!await _ctx.Database.CanConnectAsync())
        {
            StatusMessage = "Không tồn tại databse";
            return RedirectToAction("Index");
        }

        var result = await _ctx.Database.EnsureDeletedAsync();
        if (result)
        {
            StatusMessage = "Xóa database thành công";
            return RedirectToAction("Index");
        }
        StatusMessage = "Không thể xóa database";
        return RedirectToAction("Index");

    }
}