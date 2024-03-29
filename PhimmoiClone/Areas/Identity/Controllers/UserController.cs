using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Areas.Identity.Models;
using MovieApp.Areas.Identity.ViewModels;
using MovieApp.Data;

namespace MovieApp.Areas.Identity.Controllers;

[Area("Identity")]
[Route("User/[action]")]
public class UserController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly MyDbContext _ctx;
    private readonly ILogger<UserController> _logger;
    
    public UserController(
        MyDbContext ctx,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<UserController> logger)
    {
        _ctx = ctx;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
    }
    [TempData] 
    public string? StatusMessage { get; set; }
    public IActionResult Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    public async Task<IActionResult> UserDetail(string userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return NotFound();
        
        var userClaims = await _ctx.UserClaims.Where(c => c.UserId == user.Id).ToListAsync();
        var userRoles = await _userManager.GetRolesAsync(user);
        ViewData["UserClaims"] = userClaims;
        ViewData["UserRoles"] = userRoles;
        var roleClaims = new List<Claim>();
        foreach (var roleName in userRoles)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            if (role != null)
            {
                var rClaims = await _roleManager.GetClaimsAsync(role);
                roleClaims.AddRange(rClaims);
            }
        }

        ViewData["RoleClaims"] = roleClaims;
        
        return View(user);
    }
    
    
}