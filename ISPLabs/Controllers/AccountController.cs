using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Models;
using ISPLabs.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Oracle.ManagedDataAccess.Client;
using ISPLabs.Services;
using System.Data;
using System.Text;
using ISPLabs.Manager;

namespace ISPLabs.Controllers
{
    public class AccountController : Controller
    {
        private OracleConnection _conn;
        private UserManager _users;

        public AccountController()
        {
            _conn = OracleHelper.GetDBConnection();
            _conn.Open();
            _users = new UserManager(_conn);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid) {
                string error;
                if (_users.Login(model.Email, model.Password, out error))
                {
                    await Authenticate(await _users.GetByEmailAsync(model.Email));
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", error);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        var u = new User
            //        {
            //            Login = model.Login,
            //            Email = model.Email,
            //            Password = model.Password,
            //            RegistrationDate = DateTime.Now
            //        };
            //        u = users.Append(u);
            //        await Authenticate(u);
            //        return RedirectToAction("Index", "Home");
            //    }
            //    catch(Exception)
            //    {
            //        ModelState.AddModelError("", "Incorrect login/email");
            //    }
                 
            //}
            return View(model);
        }
        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        ~AccountController()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}