using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    [TempData] 
    public string? StatusMessage { get; set; }

    public IActionResult Index()
    {
        var roles = _ctx.Roles.ToList();
        return View(roles);
    }

   
    public async Task<IActionResult> RoleDetail(string? roleId)
    {
        var role = await _ctx.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        if (role == null)
        {
            StatusMessage = "Không tìm thấy role";
            return RedirectToAction("Index");
        }

        var claims = await _roleManager.GetClaimsAsync(role);
        ViewData["Claims"] = claims;
        return View(role);
    }
    

    public IActionResult AddRole()
    {
        return View(new AddRoleViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(AddRoleViewModel newRole)
    {
        if (ModelState.IsValid)
        {
            
            if (await _roleManager.RoleExistsAsync(newRole.Name))
            {
                StatusMessage = "Error: role đã tồn tại";
                return RedirectToAction("AddRole");
            }

            var identityRole = new IdentityRole()
            {
                Name = newRole.Name,
                NormalizedName = newRole.NormalizeName
            };
            
            var result = await _roleManager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                StatusMessage = $"Tạo thành công role: {newRole.Name}";
            }
            
        }
        else
        {
            StatusMessage = "Thêm role thất bại";
        }
        return RedirectToAction("AddRole");
    }

    public async Task<IActionResult> EditRole(string roleId)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        return View(role);
    }

    [HttpPost]
    public async Task<IActionResult> EditRole(IdentityRole editRole)
    {
        if (ModelState.IsValid)
        {
            
            if (await _roleManager.RoleExistsAsync(editRole.Name))
            {
                StatusMessage = "Error: role name đã tồn tại";
                return RedirectToAction("EditRole", new {roleId = editRole.Id});
            }

            
            
            var result = await _roleManager.UpdateAsync(editRole);
            if (result.Succeeded)
            {
                StatusMessage = $"Sửa role thành công";
            }
            
        }
        else
        {
            StatusMessage = "Sửa role thất bại";
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DeleteRoleConfirm(string roleId)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        if (role != null)
            return View(role);
        StatusMessage = "Không tìm thấy role";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

        if (role != null)
        {
            var result = await _roleManager.DeleteAsync(role);
            StatusMessage = result.Succeeded ? "Xóa role thành công" : "Xóa role thất bại";
        }
        else
        {
            StatusMessage = "Không tìm thấy role";
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> GetAllUser(string roleId)
    {

        var users = new List<IdentityUser>();
        var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
        if (role != null)
        {
            foreach (var user in _ctx.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                    users.Add(user);
            }
        }
        
        return View(users);
    }

    public async Task<IActionResult> AssignUserRoles(string? userId)
    {
        if (userId != null)
        {
            var user = _ctx.Users.FirstOrDefault(user => user.Id == userId);
            
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

                ViewData["AllRoles"] = allRoles;
                ViewData["UserRoles"] = userRoles;
            }

            return View(user);
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AssignUserRoles(string? userId, List<string>? selectedRoles)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user != null)
        {
            // remove all old roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            
            // add new roles
            var result = await _userManager.AddToRolesAsync(user, selectedRoles);
            StatusMessage = result.Succeeded ? "Cập nhật roles thành công" : "Không thể cập nhật user roles";
            return RedirectToAction("AssignUserRoles", new { userId = userId });
        }

        StatusMessage = "Không thể cập nhật user roles";
        return RedirectToAction("AssignUserRoles", new { userId = userId });
    }

}