using Microsoft.AspNetCore.Identity;
using MovieApp.Areas.Cinema.Models;

namespace MovieApp.Areas.Identity.Models;

public class AppUser : IdentityUser
{
    public List<Comment>? Comments { get; set; }
}