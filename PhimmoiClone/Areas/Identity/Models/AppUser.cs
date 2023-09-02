using Microsoft.AspNetCore.Identity;
using PhimmoiClone.Areas.Cinema.Models;

namespace PhimmoiClone.Areas.Identity.Models;

public class AppUser : IdentityUser
{
    public List<Comment>? Comments { get; set; }
}