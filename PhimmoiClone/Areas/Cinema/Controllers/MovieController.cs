using Microsoft.AspNetCore.Mvc;

namespace PhimmoiClone.Areas.Cinema.Controllers;

public class MovieController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }
}