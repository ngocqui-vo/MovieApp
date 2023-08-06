using Microsoft.AspNetCore.Mvc;

namespace PhimmoiClone.Areas.Cinema.Controllers;

public class GenreController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}