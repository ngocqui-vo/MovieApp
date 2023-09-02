using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Areas.Identity.Models;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Database.Controllers;

[Area("Database")]
[Route("Database/[action]")]
public class DatabaseController : Controller
{
    private readonly MyDbContext _ctx;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DatabaseController(
        MyDbContext ctx, 
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _ctx = ctx;
        _userManager = userManager;
        _roleManager = roleManager;
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

    public async Task<IActionResult> SeedAdminUserAndAdminRole()
    {
        if (await _roleManager.FindByNameAsync("admin") == null)
        {
            var adminRole = new IdentityRole()
            {
                Name = "admin"
            };
            var result = await _roleManager.CreateAsync(adminRole);
        }
        else
        {
            StatusMessage = "đã tồn tại admin role";
        }
        if (await _userManager.FindByEmailAsync("admin@gmail.com") == null)
        {
            var adminUser = new AppUser()
            {
                UserName = "admin",
                Email = "admin@gmail.com"
            };
            var result = await _userManager.CreateAsync(adminUser, "123");
            StatusMessage = result.Succeeded ? "seed thành công admin user" : "seed user thất bại";
            await _userManager.AddToRoleAsync(adminUser, "admin");

            return RedirectToAction("Index");
        }
        else
        {
            StatusMessage = string.IsNullOrEmpty(StatusMessage) 
                ? StatusMessage + ", đã tồn tại admin user" 
                : "đã tồn tại admin user";
        }

        
        return RedirectToAction("Index");
    }

}