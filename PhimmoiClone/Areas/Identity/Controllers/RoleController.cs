using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Areas.Identity.Models;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Identity.Controllers;

[Area("Identity")]
[Route("Role/[action]")]
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

    public IActionResult Index()
    {
        var roles = _roleManager.Roles.ToList();
        return View(roles);
    }

    public IActionResult AddRole()
    {
        return View(new AddRoleViewModel());
    }
    
    
    public IActionResult GetAllUser()
    {
        var users = _ctx.Users.ToList();
        return View(users);
    }

    public async Task<IActionResult> GetUser(string? username)
    {
        if (username != null)
        {
            var user = _ctx.Users.FirstOrDefault(user => user.UserName == username);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                ViewData["UserRoles"] = userRoles;
            }

            return View(user);
        }

        return NotFound();
    }
    
    
    
}