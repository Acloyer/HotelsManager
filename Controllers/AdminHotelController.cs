using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelsManager.Models;
using HotelsManager.Models.Hotels;
using HotelsManager.Models.Repository;
using HotelsManager.Models.HotelsRepository;

namespace HotelsManager.Controllers
{
    public class AdminHotelController : Controller
    {

        IHotelRepository repo;

        public AdminHotelController(IHotelRepository r)
        {
            repo = r;
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult Index()
        {
            return View("~/Views/Admin/Hotel/Index.cshtml", repo.GetHotels());
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult Details(int id)
        {
            Hotel hotel = repo.Get(id);
            if (hotel != null)
                return View(hotel);
            return NotFound();
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult CreateHotel()
        {
            return View("~/Views/Admin/Hotel/CreateHotel.cshtml");
        }

        [HttpPost]
        [Authorize(Policy = "Administrators")]
        public ActionResult CreateHotel(Hotel hotel)
        {
            repo.Create(hotel);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult EditHotel(int id)
        {
            if (repo.Get(id) == null)
            {
                return NotFound();
            }
            else
            {
                Hotel hotel = repo.Get(id);
                if (hotel != null)
                    return View("~/Views/Admin/Hotel/EditHotel.cshtml", hotel);
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Policy = "Administrators")]
        [HttpPost]
        public ActionResult EditHotel(Hotel hotel)
        {
            if (repo.Get(hotel.Id) == null)
            {
                return NotFound();
            }
            else
            {
                repo.Update(hotel);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [ActionName("DeleteHotel")]
        [Authorize(Policy = "Administrators")]
        public ActionResult ConfirmDelete(int id)
        {
            if (repo.Get(id) == null)
            {
                return NotFound();
            }
            else
            {
                Hotel hotel = repo.Get(id);
                if (hotel != null)
                {
                    return View("~/Views/Admin/Hotel/DeleteHotel.cshtml", hotel);
                }
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Policy = "Administrators")]
        public ActionResult DeleteHotel(int id)
        {
            if (repo.Get(id) == null)
            {
                return NotFound();
            }
            else
            {
                repo.Delete(id);
                return View("~/Views/Admin/Hotel/deletedhotel228.cshtml", repo.GetHotels());
            }
        }

        [HttpPost]
        [Authorize(Policy = "Administrators")]
        public ActionResult DeleteHotel1(int id)
        {
            if (repo.Get(id) == null)
            {
                return NotFound();
            }
            else
            {
                repo.Delete(id);
                return RedirectToAction("deletedhotel228");
            }
        }
        [Authorize(Policy = "Administrators")]
        public ActionResult deletedhotel228()
        {
            return View("~/Views/Admin/Hotel/deletedhotel228.cshtml", repo.GetHotels());
        }
    }
}