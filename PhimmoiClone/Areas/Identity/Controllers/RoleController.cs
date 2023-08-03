using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Identity.Controllers;

public class RoleController : Controller
{
    private readonly MyDbContext _ctx;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public RoleController(
        MyDbContext ctx,
        RoleManager<IdentityRole> roleManager, 
        UserManager<IdentityUser> userManager)
    {
        _ctx = ctx;
        _roleManager = roleManager;
        _userManager = userManager;
    }
    // GET
    public IActionResult Index()
    {
        var users = _ctx.Users.ToList();
        return View(users);
    }

    public IActionResult GetUser(string? id)
    {
        if (id != null)
        {
            var user = _ctx.Users.FirstOrDefault(user => user.Id == id);
            return View(user);
        }

        return NotFound();
    }
}