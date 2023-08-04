using Microsoft.AspNetCore.Mvc;

namespace PhimmoiClone.Areas.Database.Controllers;

public class DatabaseController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}