using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;
using System.Security.Claims;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using HotelsManager.Dtos;
using HotelsManager.Models.Repository;
using HotelsManager.Models.Users;
using HotelsManager.Models.Orders;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace HotelsManager.Controllers
{
    [Route("[controller]/[action]")]
    public class OrderController : Controller
    {
        IOrderRepository repo;
        public OrderController(IOrderRepository r)
        {
            repo = r;
        }

        [Authorize]
        public IActionResult Index()
        {
            if(repo.GetOrders(User.Identity.Name).Count > 0)
            {
                foreach(var o in repo.GetOrders(User.Identity.Name))
                {
                    Console.WriteLine(o.HotelName);
                }
                return View("~/Views/Order/Index.cshtml", repo.GetOrders(User.Identity.Name));
            }
            else
            {
                return View("~/Views/Order/NoOrders.cshtml");
            }
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            // логика удаления
            repo.Delete(id, User.Identity.Name);
            //
            return base.RedirectToAction(controllerName: "Order", actionName: "Index");
        }
    }
}