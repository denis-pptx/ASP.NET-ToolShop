using Microsoft.AspNetCore.Authorization;

namespace WEB_153503_Konchik.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        ViewData["lab2"] = "Лабораторная работа №2";

        var demoItemList = new List<DemoItem>() {
            new() { Id = 1, Name = "Denis" } ,
            new() { Id = 2, Name = "Aleksandr" },
            new() { Id = 3, Name = "Artur" },
            new() { Id = 4, Name = "Vladimir" },
            new() { Id = 5, Name = "Igor" }
        };
        var selectList = new SelectList(demoItemList, "Id", "Name");
      
        return View(selectList);
    }
}
