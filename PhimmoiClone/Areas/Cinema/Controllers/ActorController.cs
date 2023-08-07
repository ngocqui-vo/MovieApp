using Microsoft.AspNetCore.Mvc;

namespace PhimmoiClone.Areas.Cinema.Controllers;

public class ActorController : Controller
{
    public ActorController()
    {
        
    }
    public IActionResult Index()
    {
        return View();
    }
}