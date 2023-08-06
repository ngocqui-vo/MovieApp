using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhimmoiClone.Areas.Identity.Models;
using PhimmoiClone.Data;

namespace PhimmoiClone.Areas.Identity.Controllers;


[Area("Identity")]
[Route("Claim/[action]")]
public class ClaimController : Controller
{
    private readonly MyDbContext _ctx;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public ClaimController(
        MyDbContext ctx,
        RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager)
    {
        _ctx = ctx;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [TempData]
    public string? StatusMessage { get; set; }
    
    [HttpGet]
    public IActionResult AddClaim()
    {
        
        ViewData["Options"] = _ctx.Roles.Select(
            r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }
        ).ToList();
        return View(new AddClaimViewModel());
    }

    [HttpPost]
    public IActionResult AddClaim(AddClaimViewModel newClaim)
    {
        
    }
}