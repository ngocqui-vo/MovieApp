using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        StatusMessage = "";
    }
    [TempData] 
    public string StatusMessage { get; set; }
    public IActionResult Index()
    {
        return View(_ctx);
    }

    public async Task<IActionResult> CreateDatabase()
    {
        if (await _ctx.Database.CanConnectAsync())
        {
            StatusMessage = "Error: đã tồn tại databse";
            return RedirectToAction("Index");
        }

        var result = await _ctx.Database.EnsureCreatedAsync();
        if (!result)
        {
            StatusMessage = "Error: không thể tạo database";
            return RedirectToAction("Index");
        }

        StatusMessage = "Đã tạo database";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteDatabase()
    {
        if (!await _ctx.Database.CanConnectAsync())
        {
            StatusMessage = "Không tồn tại database";
            return RedirectToAction("Index");
        }

        var result = await _ctx.Database.EnsureDeletedAsync();
        if (!result)
        {
            StatusMessage = "Xóa thất bại";
            return RedirectToAction("Index");
        }

        StatusMessage = "Xóa thành công";
        return RedirectToAction("Index");
    }

}