using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelsManager.Models;
using HotelsManager.Models.Hotels;
using HotelsManager.Models.Orders;
using HotelsManager.Models.Repository;
using HotelsManager.Models.HotelsRepository;

namespace HotelsManager.Controllers
{
    public class AdminOrderController : Controller
    {

        IOrderRepository repo;

        public AdminOrderController(IOrderRepository r)
        {
            repo = r;
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult Index()
        {
            return View("~/Views/Admin/Order/Index.cshtml", repo.GetOrders());
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult Details(int id)
        {
            Order order = repo.Get(id);
            if (order != null)
                return View(order);
            return NotFound();
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult CreateOrder()
        {
            return View("~/Views/Admin/Order/CreateOrder.cshtml");
        }

        [HttpPost]
        [Authorize(Policy = "Administrators")]
        public ActionResult CreateOrder(Order order)
        {
            repo.Create(order);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult EditOrder(int id)
        {
            if (repo.Get(id) == null)
            {
                return NotFound();
            }
            else
            {
                Order order = repo.Get(id);
                if (order != null)
                    return View("~/Views/Admin/Order/EditOrder.cshtml", order);
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Policy = "Administrators")]
        [HttpPost]
        public ActionResult EditOrder(Order order)
        {
            if (repo.Get(order.Id) == null)
            {
                return NotFound();
            }
            else
            {
                repo.Update(order);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [ActionName("DeleteOrder")]
        [Authorize(Policy = "Administrators")]
        public ActionResult ConfirmDelete(int id)
        {
            if (repo.Get(id) == null)
            {
                return NotFound();
            }
            else
            {
                Order order = repo.Get(id);
                if (order != null)
                {
                    return View("~/Views/Admin/Order/DeleteOrder.cshtml", order);
                }
                return NotFound();
            }
        }
        [HttpPost]
        [Authorize(Policy = "Administrators")]
        public ActionResult DeleteOrder(int id)
        {
            if (repo.Get(id) == null)
            {
                return NotFound();
            }
            else
            {
                repo.Delete(id);
                return RedirectToAction("Index");
            }
        }
    }
}