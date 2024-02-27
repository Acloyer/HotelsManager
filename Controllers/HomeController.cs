namespace HotelsManager.Controllers;

using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelsManager.Models;
using HotelsManager.Models.Users;
using HotelsManager.Models.Repository;

public class HomeController : Controller
{
    IUserRepository repo;
    public HomeController(IUserRepository r)
    {
        repo = r;
    }

    public ActionResult Index()
    {
        return View(repo.GetUsers());
    }

    public ActionResult Loading()
    {
        return View("~/Views/Home/Loading.cshtml");
    }

    [HttpGet]
    [Authorize]
    public IActionResult Home()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}