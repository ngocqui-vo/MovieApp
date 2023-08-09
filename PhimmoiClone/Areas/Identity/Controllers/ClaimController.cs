using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhimmoiClone.Areas.Identity.Models;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Identity.Controllers;

[Area("Identity")]
[Route("Claim/[action]")]
public class ClaimController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly MyDbContext _ctx;
    private readonly ILogger<UserController> _logger;
    
    public ClaimController(
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
    
    public async Task<IActionResult> AddUserClaim(string userId)
    {
        var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == userId);
        ViewData["User"] = user;
        return View(new AddClaimUserViewModel());
    }
    [HttpPost]
    public async Task<IActionResult> AddUserClaim(AddClaimUserViewModel addClaimModel)
    {
        if (ModelState.IsValid)
        {
            var newClaim = new IdentityUserClaim<string?>()
            {
                UserId = addClaimModel.UserId,
                ClaimType = addClaimModel.Type,
                ClaimValue = addClaimModel.Value
            };
            await _ctx.UserClaims.AddAsync(newClaim);
            await _ctx.SaveChangesAsync();
            StatusMessage = "thêm thành công claim";
            
        }
        else
        {
            StatusMessage = "Error: thêm claim thất bại";
        }
        var user = await _ctx.Users.FirstOrDefaultAsync(r => r.Id == addClaimModel.UserId);
        return RedirectToAction("AddUserClaim", new {userId = user?.Id});
       
    }
    public async Task<IActionResult> AddRoleClaim(string roleId)
    {
        var role = await _ctx.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        ViewData["Role"] = role;
        return View(new AddClaimRoleViewModel());
    }
    [HttpPost]
    public async Task<IActionResult> AddRoleClaim(AddClaimRoleViewModel addClaimModel)
    {
        if (ModelState.IsValid)
        {
            var newClaim = new IdentityRoleClaim<string?>()
            {
                RoleId = addClaimModel.RoleId,
                ClaimType = addClaimModel.Type,
                ClaimValue = addClaimModel.Value
            };
            await _ctx.RoleClaims.AddAsync(newClaim);
            await _ctx.SaveChangesAsync();
            StatusMessage = "thêm thành công claim";
            
        }
        else
        {
            StatusMessage = "Error: thêm claim thất bại";
        }
        var role = await _ctx.Roles.FirstOrDefaultAsync(r => r.Id == addClaimModel.RoleId);
        return RedirectToAction("AddRoleClaim", new {roleId = role?.Id});
       
    }
}