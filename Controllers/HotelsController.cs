
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelsManager.Models;

namespace HotelsManager.Controllers;

public class HotelsController : Controller
{
    private readonly Hotels _context;

    public HotelsController(Hotels context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Name.ToList();
        return View(products);
    }
}
