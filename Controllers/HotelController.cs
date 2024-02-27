using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelsManager.Models;
using HotelsManager.Models.Hotels;
using HotelsManager.Models.Orders;
using HotelsManager.Models.HotelsRepository;

namespace HotelsManager.Controllers
{
    public class HotelController : Controller
    {
        IHotelRepository repo;

        public HotelController(IHotelRepository r)
        {
            repo = r;
        }

        [Authorize]
        [Route("Hotel/Index/{id?}")]
        public IActionResult Index(int id)
        {
            if (id != null && id != 0)
            {
                var user = User;
                try
                {
                    var count = 0;
                    foreach(var i in repo.GetHotels()) { 
                        Console.WriteLine(i.Id);
                        count++;
                        if (i.Id == id)
                            id = count;
                    }
                    Console.WriteLine("count: " + count);
                    Console.WriteLine("id: " + id);
                    Console.WriteLine(">>" + repo.GetHotels()[id - 1].Stars);
                    Order new_order = new Order(username: user.Identity.Name,
                                                hotelId: repo.GetHotels()[id - 1].Id,
                                                hotelName: repo.GetHotels()[id - 1].Name,
                                                hotelStars: repo.GetHotels()[id - 1].Stars,
                                                hotelPrice: repo.GetHotels()[id - 1].Price,
                                                hotelCurrency: repo.GetHotels()[id - 1].Currency);
                    repo.CreateOrder(new_order);
                }
                catch (Exception e){
                    Console.WriteLine(e.Message);
                    return View("~/Views/Hotel/Index.cshtml", repo.GetHotels());
                }
                return View("~/Views/Hotel/Index.cshtml", repo.GetHotels());
                /*return base.RedirectToAction(controllerName: "Hotel", actionName: "SuccessOrder");*/
            }
            else // если зашел по кнопке
                return View("~/Views/Hotel/Index.cshtml", repo.GetHotels());
        }

        public IActionResult ErrorId()
        {
            return View();
        }

        public IActionResult SuccessOrder()
        {
            return View();
        }
    }
}