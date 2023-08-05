using Microsoft.AspNetCore.Mvc;

namespace PhimmoiClone.Areas.Movie.Controllers;

public class MovieController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }
}