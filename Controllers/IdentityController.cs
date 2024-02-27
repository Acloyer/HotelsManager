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
/*using HotelsManager.Models.ProfileViewModel;*/
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace HotelsManager.Controllers
{
    [Route("[controller]/[action]")]
    public class IdentityController : Controller
    {
        IUserRepository repo;
        string Password = string.Empty;
        public IdentityController(IUserRepository r)
        {
            repo = r;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Forbidden(string? returnUrl) {
            ViewData["returnUrl"] = returnUrl;

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl) {
            base.ViewData["returnUrl"] = returnUrl;

            return base.View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Home(string? returnUrl)
        {
            base.ViewData["returnUrl"] = returnUrl;

            return base.View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Registration(string? returnUrl) {
            if (!User.Identity.IsAuthenticated)
            {
                base.ViewData["returnUrl"] = returnUrl;

                return base.View();
            }
            else
                return base.RedirectToAction(controllerName: "Home", actionName: "Index");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromForm] LoginDto loginDto) {
            if (!User.Identity.IsAuthenticated)
            {
                UserRepository dataAccess = new UserRepository(@"Data Source=LAPTOP-8U7UGFTE;Initial Catalog=HotelsDB;
									    Integrated Security=SSPI;TrustServerCertificate=True;");
                List<HotelsManager.Models.Users.User> users = dataAccess.GetUsers();
                foreach(var user in users)
                {
                    if(loginDto.Login == user.Name)
                    {
                        if(loginDto.Password == user.Password)
                        {
                            var roleName = loginDto.Login.Contains("ADM_")
                            ? "admin"
                            : "user";

                            var claims = new List<Claim> {
                                new(ClaimTypes.Name, loginDto.Login),
                                new("Password", loginDto.Password),
                                new(ClaimTypes.Role, roleName),
                                new("creation_date_utc", DateTime.UtcNow.ToString()),
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            base.HttpContext.SignInAsync(
                            scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                            principal: new ClaimsPrincipal(claimsIdentity)
                        );

                        if (string.IsNullOrWhiteSpace(loginDto.ReturnUrl))
                            return base.RedirectToAction(controllerName: "Home", actionName: "Index");
                        }
                    }
                }
                return View();
            }
            else
                return base.RedirectToAction(controllerName: "Home", actionName: "Index");

        }
        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            var password = repo.GetPassword(repo.GetId(User.Identity.Name).Id).Password;
            var orders = repo.GetOrdersCount(User.Identity.Name);
            ProfileViewModel data = new ProfileViewModel(User.Identity.Name, password, orders);
            return View(data);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Registration([FromForm] LoginDto loginDto) {
            if (!User.Identity.IsAuthenticated)
            {
                UserRepository dataAccess = new UserRepository(@"Data Source=LAPTOP-8U7UGFTE;Initial Catalog=HotelsDB;
									Integrated Security=SSPI;TrustServerCertificate=True;");
                List<HotelsManager.Models.Users.User> users = dataAccess.GetUsers();
                var lastId = 1;
                if (users != null)
                {
                    if (users.Count > 0)
                    {
                        lastId = users[users.Count - 1].Id;
                    }
                }
                var canRegister = false;
                string login = loginDto.Login;
                string password = loginDto.Password;

                /*if (Regex.IsMatch(login, @"^[a-zA-Z0-9_]{4,19}$") && Regex.IsMatch(password, @"^[a-zA-Z0-9_]{6,19}$"))
                {*/
                foreach (var user in users)
                {
                    if (user.Name != loginDto.Login)
                    {
                        canRegister = true;
                    }
                    else
                    {
                        canRegister = false;
                        break;
                        /*return View();*/
                    }
                }

                if (canRegister)
                {
                    HotelsManager.Models.Users.User user1 = new HotelsManager.Models.Users.User(lastId + 1, loginDto.Login, loginDto.Password);
                    try
                    {
                        repo.Create(user1);
                    }
                    catch (Exception ex)
                    {
                        // Выводите информацию об ошибке или логируйте ее
                    }
                    var roleName = login.Contains("ADM_")
                            ? "admin"
                            : "user";

                    var claims = new List<Claim> {
                            new(ClaimTypes.Name, loginDto.Login),
                            new("Password", loginDto.Password),
                            new(ClaimTypes.Role, roleName),
                            new("creation_date_utc", DateTime.UtcNow.ToString()),
                        };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    base.HttpContext.SignInAsync(
                        scheme: CookieAuthenticationDefaults.AuthenticationScheme,
                        principal: new ClaimsPrincipal(claimsIdentity)
                    );

                    if (string.IsNullOrWhiteSpace(loginDto.ReturnUrl))
                        return base.RedirectToAction(controllerName: "Home", actionName: "Index");

                    return base.RedirectPermanent(loginDto.ReturnUrl);
                }
                return View();
            }
            else
                return base.RedirectToAction(controllerName: "Home", actionName: "Index");
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> LogOut() {
            await base.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return base.RedirectToAction(actionName: "Login");
        }
    }
}