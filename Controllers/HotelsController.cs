namespace HotelsManager.Controllers;

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using HotelsManager.Models;

public class HotelsController : Controller
{
    public IActionResult Profile(int id)
    {
        var hotelProfileJson = System.IO.File.ReadAllText("Assets/hotels_profile.json");

        var jsonOptions = new JsonSerializerOptions() {
            PropertyNameCaseInsensitive = true,
        };
        var profiles = JsonSerializer.Deserialize<IEnumerable<Hotel>>(hotelProfileJson, jsonOptions);

        var profile = profiles?.FirstOrDefault(p => p.Id == id);

        if(profile is null)
            return base.NotFound();

        return View(profile);
    }
}