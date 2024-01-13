using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApp.Areas.Cinema.Models;
using MovieApp.Areas.Cinema.ViewModel;
using MovieApp.Areas.Identity.Models;
using MovieApp.Data;

namespace MovieApp.Controllers;


public class CommentController : Controller
{
    private readonly MyDbContext _ctx;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public CommentController(MyDbContext ctx, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _ctx = ctx;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    

    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody]CommentViewModel vm)
    {
        if (ModelState.IsValid)
        {
            var comment = new Comment()
            {
                UserId = vm.UserId,
                MovieId = vm.MovieId,
                Message = vm.Message
            };
            await _ctx.Comments.AddAsync(comment);
            await _ctx.SaveChangesAsync();
            return Ok("thêm comment thành công");
        }
        return BadRequest("thêm comment thất bại");
    }

    [HttpGet]
    public async Task<IActionResult> LoadComments(int movieId)
    {
        var comments = await _ctx.Comments
            .Include(c => c.User)
            .Where(c => c.MovieId == movieId)
            .OrderByDescending(c => c.Created)
            .ToListAsync();
        var json = comments
            .Select(comment => new {
                id = comment?.Id,
                userName = comment?.User?.UserName, 
                message = comment?.Message
            });
        
        return Json(json);
    }

    [HttpGet]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var isSignedIn = _signInManager.IsSignedIn(HttpContext.User);
        if (!isSignedIn) 
            return BadRequest("xóa thất bại");
        
        var userId = _userManager.GetUserId(User);
        var comment = await _ctx.Comments
            .FirstOrDefaultAsync(c => c.Id == commentId);
        if (comment == null || comment?.UserId != userId) 
            return BadRequest("xóa thất bại");
        
        _ctx.Comments.Remove(comment);
        var kq = await _ctx.SaveChangesAsync();
        return Ok("xóa thành công");

    }
    
}