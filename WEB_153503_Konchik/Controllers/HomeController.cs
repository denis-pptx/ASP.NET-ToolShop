using Microsoft.AspNetCore.Mvc;

namespace WEB_153503_Konchik.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
