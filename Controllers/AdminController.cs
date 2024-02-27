using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HotelsManager.Models;
using HotelsManager.Models.Users;
using HotelsManager.Models.Repository;
using HotelsManager.Dtos;

namespace HotelsManager.Controllers
{
    public class AdminController : Controller
    {

        IUserRepository repo;

        public AdminController(IUserRepository r)
        {
            repo = r;
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult UserList()
        {
            return View("~/Views/Admin/User/Index.cshtml", repo.GetUsers());
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult Details(int id)
        {
            User user = repo.Get(id);
            if (user != null)
                return View(user);
            return NotFound();
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult CreateUser()
        {
            return View("~/Views/Admin/User/CreateUser.cshtml");
        }

        [HttpPost]
        [Authorize(Policy = "Administrators")]
        public ActionResult CreateUser(User user)
        {
            repo.Create(user);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = "Administrators")]
        public ActionResult EditUser(int id)
        {
            if (repo.Get(id) == null)
            {
                return NotFound();
            }
            else
            {
                if (!(User.Identity.Name == repo.Get(id).Name))
                {
                    User user = repo.Get(id);
                    if (user != null)
                        return View("~/Views/Admin/User/EditUser.cshtml", user);
                    return NotFound();
                }
                else
                {
                    return RedirectToAction("UserList");
                }
            }
        }

        public ActionResult LoginToUserAccount(string rUrl, string name, string password)
        {
            LoginDto loginDto = new LoginDto(rUrl, name, password);
            return RedirectToAction("~/Controllers/IdentityController.cs/Login");
        }
        [HttpPost]
        [Authorize(Policy = "Administrators")]
        [HttpPost]
        public ActionResult EditUser(User user)
        {
            if (repo.Get(user.Id) == null)
            {
                return NotFound();
            }
            else {
                if (!(User.Identity.Name == repo.Get(user.Id).Name))
                {
                    repo.Update(user);
                    return RedirectToAction("UserList");
                }
                else
                {
                    return RedirectToAction("UserList");
                }
            }
        }

        [HttpGet]
        [ActionName("DeleteUser")]
        [Authorize(Policy = "Administrators")]
        public ActionResult ConfirmDelete(int id)
        {
            if(repo.Get(id) == null)
            {
                return NotFound();
            }
            else
            {
                if (!(User.Identity.Name == repo.Get(id).Name))
                {
                    User user = repo.Get(id);
                    if (user != null)
                    {
                        return View("~/Views/Admin/User/DeleteUser.cshtml", user);
                    }
                    return NotFound();
                }
                else
                {
                    return RedirectToAction("UserList");
                }
            }
        }
        [HttpPost]
        [Authorize(Policy = "Administrators")]
        public ActionResult DeleteUser(int id)
        {
            if (repo.Get(id) == null)
            {
                return NotFound();
            }
            else
            {
                if (!(User.Identity.Name == repo.Get(id).Name)){
                    repo.Delete(id);
                    return RedirectToAction("UserList");
                }
                else
                {
                    return RedirectToAction("UserList");
                }
            }
        }
    }
}