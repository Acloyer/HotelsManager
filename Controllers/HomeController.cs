namespace HotelsManager.Controllers;

using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelsManager.Models;


public class HomeController : Controller
{
    // public HomeController()
    // {
    // // System.Console.WriteLine("HomeController created!");
    // }

    public IActionResult Index()
    {
        var hotelsJson = System.IO.File.ReadAllText("Assets/hotels_profile.json");
        var hotels = JsonSerializer.Deserialize<IEnumerable<Hotel>>(hotelsJson);
        

        return base.View(hotels);
    }
    [HttpGet]
    [Authorize(Policy = "Administrators")]
    public IActionResult Secret()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}