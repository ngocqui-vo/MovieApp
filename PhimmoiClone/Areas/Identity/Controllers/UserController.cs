using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Areas.Identity.Models;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Identity.Controllers;

[Area("Identity")]
[Route("User/[action]")]
public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly MyDbContext _ctx;
    private readonly ILogger<UserController> _logger;
    
    public UserController(
        MyDbContext ctx,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<UserController> logger)
    {
        _ctx = ctx;
        _userManager = userManager;
        _signInManager = signInManager;
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
        if (user != null)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            ViewData["UserClaims"] = userClaims;
            ViewData["UserRoles"] = userRoles;
        }

        return View(user);
    }
    
    
}