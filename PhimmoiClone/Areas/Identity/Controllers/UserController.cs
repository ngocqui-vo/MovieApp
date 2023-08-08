using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PhimmoiClone.Areas.Identity.Controllers;

public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
       
    private readonly ILogger<UserController> _logger;
    
    public UserController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ILogger<UserController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
            
        _logger = logger;
        
    }
    
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
            var userClaims = _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            ViewData["UserClaims"] = userClaims;
            ViewData["UserRoles"] = userRoles;
        }

        return View(user);
    }
}