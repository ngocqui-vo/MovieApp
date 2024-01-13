using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Areas.Identity.Models;
using MovieApp.Areas.Identity.ViewModels;
using MovieApp.Areas.Identity.ViewModels;
using MovieApp.Data;

namespace MovieApp.Areas.Identity.Controllers;

[Area("Identity")]
[Route("Claim/[action]")]
public class ClaimController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly MyDbContext _ctx;
    private readonly ILogger<UserController> _logger;
    
    public ClaimController(
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
    [HttpPost]
    public async Task<IActionResult> DeleteUserClaim(int claimId, string userId)
    {
        var userClaim = await _ctx.UserClaims.FirstOrDefaultAsync(c => c.Id == claimId);
        if (userClaim != null)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            
           await _userManager
                .RemoveClaimAsync(user, new Claim(userClaim.ClaimType, userClaim.ClaimValue));
           StatusMessage = "vừa xóa claim";
        }
        return RedirectToAction("UserDetail", "User", new { userId = userId });
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
    
    [HttpPost]
    public async Task<IActionResult> DeleteRoleClaim(int claimId, string roleId)
    {
        var roleClaim = await _ctx.RoleClaims.FirstOrDefaultAsync(c => c.Id == claimId);
        if (roleClaim != null)
        {
            var role = await _ctx.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            
            await _roleManager
                .RemoveClaimAsync(role, new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
            StatusMessage = "vừa xóa claim";
        }
        return RedirectToAction("RoleDetail", "Role", new { roleId = roleId });
    } 
}