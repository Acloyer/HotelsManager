using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelsManager.Models;
using System.Text.Json;

namespace HotelsManager.Controllers;

using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using HotelsManager.Dtos;
using HotelsManager.Models;

public class HomeController : Controller
{
    public HomeController()
    {
        System.Console.WriteLine("HomeController created!");
    }

    public IActionResult MyMethod(int? id, string? name = "unknown") {
        return Ok(new {
            id, name
        });
    }

    [HttpGet]
    public IActionResult Login() {
        return base.View();
    }
    
    [HttpPost]
    public IActionResult Login(LoginDto loginDto) {
        System.Console.WriteLine($"{loginDto.Login} {loginDto.Password}");

        return RedirectToAction("Index");
    }



    public IActionResult Index()
    {
        var hotelsJson = System.IO.File.ReadAllText("Assets/hotels_profile.json");
        var hotels = JsonSerializer.Deserialize<IEnumerable<Hotel>>(hotelsJson);
        

        return base.View(hotels);
    }

    public IActionResult Privacy()
    {
        return base.View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}